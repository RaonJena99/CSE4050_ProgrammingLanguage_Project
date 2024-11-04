namespace FMinus

open AST

// Type.infer() must raise this if the input program seems to have a type error.
exception TypeError

// The types available in our F- language.
type Type =
  | Int
  | Bool
  | TyVar of string
  | Func of Type * Type

type TypeEnv = Map<string, Type>

module Type =

  // Convert the given 'Type' to a string.
  let rec toString (typ: Type): string =
    match typ with
    | Int -> "int"
    | Bool -> "bool"
    | TyVar s -> s
    | Func (t1, t2) -> sprintf "(%s) -> (%s)" (toString t1) (toString t2)
  
  let extend (x: string) (t: Type) (env: TypeEnv) : TypeEnv =
    Map.add x t env

  let lookup (x: string) (env: TypeEnv) : Type =
    match Map.tryFind x env with
    | Some t -> t
    | None -> raise TypeError

  let rec occurs (v: string) (t: Type) : bool =
    match t with
    | TyVar x -> x = v
    | Func (t1, t2) -> occurs v t1 || occurs v t2
    | _ -> false

  let rec unify (t1: Type) (t2: Type) : (string * Type) list =
    match t1, t2 with
    | Int, Int -> []
    | Bool, Bool -> []
    | TyVar v, t | t, TyVar v when t <> TyVar v ->
      if occurs v t then raise TypeError else [(v, t)]
    | Func (a1, r1), Func (a2, r2) ->
      let s1 = unify a1 a2
      let s2 = unify (apply_subst s1 r1) (apply_subst s1 r2)
      s1 @ s2
    | _ -> raise TypeError

  and apply_subst (subst: (string * Type) list) (t: Type) : Type =
    match t with
    | TyVar v -> 
      match List.tryFind (fun (x, _) -> x = v) subst with
      | Some (_, t) -> t
      | None -> t
    | Func (t1, t2) -> Func (apply_subst subst t1, apply_subst subst t2)
    | _ -> t

  let rec infer_exp (exp: Exp) (env: TypeEnv) : Type * (string * Type) list =
      match exp with
      | Num _ -> (Int, [])
      | True | False -> (Bool, [])
      | Var x -> (lookup x env, [])

      | Neg e -> 
        let (t, s) = infer_exp e env
        let s' = unify t Int
        (Int, s @ s')

      | Add (e1, e2) | Sub (e1, e2) ->
        let (t1, s1) = infer_exp e1 env
        let (t2, s2) = infer_exp e2 env
        let s3 = unify t1 Int
        let s4 = unify t2 Int
        (Int, s1 @ s2 @ s3 @ s4)

      | LessThan (e1, e2) | GreaterThan (e1, e2) ->
        let (t1, s1) = infer_exp e1 env
        let (t2, s2) = infer_exp e2 env
        let s3 = unify t1 Int
        let s4 = unify t2 Int
        (Bool, s1 @ s2 @ s3 @ s4)

      | Equal (e1, e2) | NotEq (e1, e2) ->
        let (t1, s1) = infer_exp e1 env
        let (t2, s2) = infer_exp e2 env
        let s3 = unify t1 t2
        (Bool, s1 @ s2 @ s3)

      | IfThenElse (e1, e2, e3) ->
        let (t1, s1) = infer_exp e1 env
        let (t2, s2) = infer_exp e2 env
        let (t3, s3) = infer_exp e3 env
        let s4 = unify t1 Bool
        let s5 = unify t2 t3
        (t2, s1 @ s2 @ s3 @ s4 @ s5)

      | LetIn (x, e1, e2) ->
        let (t1, s1) = infer_exp e1 env
        let env' = extend x t1 env 
        let (t2, s2) = infer_exp e2 env'
        (t2, s1 @ s2)

      | LetFunIn (f, x, e1, e2) ->
        let ta = TyVar "a"
        let env' = extend x ta env 
        let (tr, s1) = infer_exp e1 env'
        let env'' = extend f (Func (apply_subst s1 ta, tr)) env
        let (t2, s2) = infer_exp e2 env''
        (t2, s1 @ s2)

      | LetRecIn (f, x, e1, e2) ->
        let ta = TyVar "a"
        let tr = TyVar "r"
        let env' = extend x ta (extend f (Func (ta, tr)) env)
        let (t1, s1) = infer_exp e1 env'
        let s2 = unify tr t1
        let env'' = extend f (Func (apply_subst (s1 @ s2) ta, apply_subst (s1 @ s2) tr)) env
        let (t2, s3) = infer_exp e2 env''
        (t2, s1 @ s2 @ s3)

      | Fun (x, e) ->
        let ta = TyVar "a"
        let env' = extend x ta env 
        let (tr, s) = infer_exp e env'
        (Func (apply_subst s ta, tr), s)

      | App (e1, e2) ->
        let (t1, s1) = infer_exp e1 env
        let (t2, s2) = infer_exp e2 env
        let tr = TyVar "r"
        let s3 = unify (apply_subst s2 t1) (Func (t2, tr))
        (apply_subst s3 tr, s1 @ s2 @ s3)

  let infer (prog: Program) : Type =
    let (t, s) = infer_exp prog Map.empty
    apply_subst s t

module FMinus

open AST
open Types

// Evaluate expression into a value, under the given environment.
let rec evalExp (exp: Exp) (env: Env) : Val =
  match exp with
  | Num i -> Int i

  | True -> Bool true

  | False -> Bool false

  | Var i ->
      Map.find i env

  | Neg i -> 
      let l = evalExp i env
      match l with
      |Int v -> Int(-v)
      | _ -> raise UndefinedSemantics

  | Add (e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 -> Int(n1+n2)
      | _ -> raise UndefinedSemantics

  | Sub(e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 -> Int(n1-n2)
      | _ -> raise UndefinedSemantics

  | LessThan(e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 < n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | GreaterThan(e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 > n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | Equal(e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 = n2) then Bool true else Bool false
      |Bool n1, Bool n2 ->
        if(n1 = n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | NotEq(e1,e2) ->
      let n1 = evalExp e1 env
      let n2 = evalExp e2 env
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 <> n2) then Bool true else Bool false
      |Bool n1, Bool n2 ->
        if(n1 <> n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | IfThenElse(e1,e2,e3) ->
      let n1 = evalExp e1 env
      match n1 with
      |Bool true -> evalExp e2 env
      |Bool false -> evalExp e3 env
      | _ -> raise UndefinedSemantics

  | LetIn(v,e1,e2) ->
      let newenv = Map.add v (evalExp e1 env) env
      evalExp e2 newenv

  | LetFunIn(v1,v2,e1,e2) ->
      let newenv = Map.add v1 (Func(v2,e1,env)) env
      evalExp e2 newenv

  | LetRecIn(v1,v2,e1,e2) ->
      let newenv = Map.add v1 (RecFunc(v1,v2,e1,env)) env
      evalExp e2 newenv

  | Fun(v,e) -> Func (v,e,env)

  | App(e1,e2) ->
      let v1 = evalExp e1 env
      let v2 = evalExp e2 env
      match v1 with
      |Func(v,e,env) -> 
          let newenv = Map.add v v2 env
          evalExp e newenv
      |RecFunc(f,v,e,env) ->
          let newenv = Map.add v v2 (Map.add f (RecFunc(f,v,e,env)) env)
          evalExp e newenv
      | _ -> raise UndefinedSemantics
  
// Note: You may define more functions.

// The program starts execution with an empty environment. Do not fix this code.
let run (prog: Program) : Val =
  evalExp prog Map.empty

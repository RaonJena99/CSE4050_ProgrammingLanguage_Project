module CMinusPtr

open AST
open Types

// Evaluate expression into a value, under the given memory.
let rec evalExp (exp: Exp) (mem: Mem) : Val =
  match exp with
  | Num i -> Int i
  | True -> Bool true
  | False -> Bool false

  | LV i ->
      match i with
      | Var var -> Map.find var mem
      | Deref deref -> 
        match evalExp deref mem with
        | Loc loc -> Map.find loc mem
        | _ -> raise UndefinedSemantics 

  | AddrOf i -> Loc i
    
  | Add(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 -> Int(n1+n2)
      | _ -> raise UndefinedSemantics

  | Sub(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 -> Int(n1-n2)
      | _ -> raise UndefinedSemantics

  | LessThan(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 < n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | GreaterThan(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 > n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | Equal(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 = n2) then Bool true else Bool false
      |Bool n1, Bool n2 ->
        if(n1 = n2) then Bool true else Bool false
      |Loc n1, Loc n2 -> 
        if(n1 = n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics

  | NotEq(e1, e2) ->
      let n1 = evalExp e1 mem
      let n2 = evalExp e2 mem
      match n1,n2 with
      |Int n1, Int n2 ->
        if(n1 <> n2) then Bool true else Bool false
      |Bool n1, Bool n2 ->
        if(n1 <> n2) then Bool true else Bool false
      |Loc n1, Loc n2 -> 
        if(n1 <> n2) then Bool true else Bool false
      | _ -> raise UndefinedSemantics
      
and evalLVal (lval: LVal) (mem: Mem) : VarName =
  match lval with
  | Var i -> i
  | Deref i ->
      match evalExp i mem with
        | Loc loc -> loc 
        | _ -> raise UndefinedSemantics 

// Execute a statement and return the updated memory.
let rec exec (stmt: Stmt) (mem: Mem) : Mem =
  match stmt with
  | NOP -> mem // NOP does not change the memory.

  | Assign(l,e)->
      let lv = evalLVal l mem
      match lv with
      | lv-> Map.add lv (evalExp e mem) mem

  | Seq(s1, s2)->
      let mem1 = exec s1 mem
      exec s2 mem1
      
  | If(e, s1, s2)->
      match  evalExp e mem with
      |Bool true -> exec s1 mem
      |Bool false -> exec s2 mem
      | _ -> raise UndefinedSemantics
      
  | While(e,s)->
    let rec loop mem =
      match  evalExp e mem with
      |Bool true ->
          let mem1 = exec s mem
          loop mem1
      |Bool false -> mem
      | _ -> raise UndefinedSemantics
    loop mem


// The program starts execution with an empty memory. Do NOT fix this function.
let run (prog: Program) : Mem =
  exec prog Map.empty

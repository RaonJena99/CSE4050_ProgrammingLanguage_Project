# CSE4050 Programming Language Labs

This repository contains four lab assignments for **CSE4050: Programming Language** at Sogang University. Each lab explores fundamental concepts in programming languages, focusing on F# for problem-solving, interpreter implementation, and type inference. Below is a detailed breakdown of each lab.

## Table of Contents
1. [Lab 1: Warm-Up Exercise](#lab-1-warm-up-exercise)
2. [Lab 2: C- Interpreter](#lab-2-c--interpreter)
3. [Lab 3: F- Interpreter](#lab-3-f--interpreter)
4. [Lab 4: F- Type System](#lab-4-f--type-system)

---

## Lab 1: Warm-Up Exercise

### Overview
The **Warm-Up Exercise** is designed to introduce students to F# programming through a set of problems that cover basic list manipulations and recursive functions. It serves as an introduction to the lab structure, F# syntax, and the self-grading script.

### Structure
- **P1–P7 Directories**: Each directory contains a problem file (`P*.fs`), a test file (`Main.fs`), and a project file (`P*.fsproj`).
- **check.py**: A script that provides automatic grading feedback.
- **config**: Configuration file for grading.

### Problem Description
Each problem file includes a function with a specified task, such as reversing a list, calculating sums, or performing list operations. Constraints are provided in comments, and students are expected to follow these constraints strictly.

### Execution
1. **Setup**: Extract `Lab1.tgz` on `cspro2.sogang.ac.kr`.
2. **Compilation**:
   ```bash
   dotnet build -o out
   ```
3. **Testing**:
   - Execute `check.py` for self-assessment.
   - Add additional test cases to verify function correctness.

### Submission
Submit `P1.fs` to `P7.fs` as individual files directly to Cyber Campus. Ensure that each file compiles without errors.

---

## Lab 2: C- Interpreter

### Overview
In **Lab 2**, students build an interpreter for an imperative language called **C-**, which is similar to C. The project focuses on evaluating expressions and executing statements, including variables, conditionals, and loops.

### Structure
- **CMinus Directory**: Contains the core files for the C- language:
  - `AST.fs`: Defines the abstract syntax tree.
  - `CMinus.fs`: Main implementation file for semantic evaluation.
  - `Types.fs`: Type definitions for values and memory.
  - `Main.fs`: Driver file for running the interpreter.
- **CMinusPtr Directory**: Adds pointer support to C-.

### Tasks
1. **C- Interpreter (CMinus.fs)**: Implement functions to evaluate expressions (`evalExp`) and execute statements (`exec`).
2. **Pointer Support (CMinusPtr.fs)**: Extend C- to support pointer operations like dereferencing and address referencing.

### Execution
1. **Setup**: Extract `Lab2.tgz` on `cspro2.sogang.ac.kr`.
2. **Build and Run**:
   ```bash
   dotnet build -o out
   ./out/CMinus testcase/tc-1
   ```

### Submission
Submit `CMinus.fs` and `CMinusPtr.fs` files directly to Cyber Campus. Each file must compile correctly; otherwise, no points will be awarded.

---

## Lab 3: F- Interpreter

### Overview
The **F- Interpreter** lab focuses on interpreting an expression-based functional language called **F-**, similar to ML-style languages. Students implement semantic rules for expressions including functions, recursion, and conditional logic.

### Structure
- **FMinus Directory**:
  - `AST.fs`: Defines the syntax of the F- language.
  - `FMinus.fs`: Main file where the interpreter is implemented.
  - `Types.fs`: Defines types for environment and values.
  - `Main.fs`: Driver to run the F- interpreter.

### Tasks
1. **Expression Evaluation (FMinus.fs)**: Implement `evalExp`, the main function responsible for evaluating F- expressions based on semantic rules.
2. **Semantic Rules**: Support for expressions like `let`, `if`, and function applications with closures and recursive functions.

### Execution
1. **Setup**: Extract `Lab3.tgz` on `cspro2.sogang.ac.kr`.
2. **Build and Run**:
   ```bash
   dotnet build -o out
   ./out/FMinus testcase/tc-1
   ```

### Submission
Submit `FMinus.fs` directly to Cyber Campus. Ensure that the program passes all test cases and raises exceptions correctly for undefined semantics.

---

## Lab 4: F- Type System

### Overview
In **Lab 4**, students extend the F- interpreter by implementing a static type inference system. The goal is to assign types to expressions based on typing rules and detect type errors where applicable.

### Structure
- **FMinusType Directory**:
  - `AST.fs`: Defines syntax for F- expressions.
  - `TypeSystem.fs`: Main file to implement type inference.
  - `Main.fs`: Driver file to run the type inferencer.

### Tasks
1. **Type Inference (TypeSystem.fs)**: Implement `Type.infer`, which assigns types to expressions following type rules for F-.
2. **Type Checking**: Handle overloaded operators, type variables, and recursive function types, raising `TypeError` for invalid types.

### Execution
1. **Setup**: Extract `Lab4.tgz` on `cspro2.sogang.ac.kr`.
2. **Build and Run**:
   ```bash
   dotnet build -o out
   ./out/FMinusType testcase/tc-1
   ```

### Submission
Submit `TypeSystem.fs` to Cyber Campus. Files must compile successfully to receive full credit.

---

Each lab includes a self-grading script (`check.py`) to assist in verifying function correctness. Follow the guidelines for each lab to ensure proper grading.

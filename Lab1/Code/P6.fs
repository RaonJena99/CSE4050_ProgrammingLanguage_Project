module P6

/// From list 'l', find the element that appears most frequently in the list,
/// and return how many times it appears. If the input list is empty, return 0.
let rec countMostFrequent (l: List<'a>) : int =
  match l with
  |[] -> 0
  |_ ->
      List.groupBy id l
      |> List.map (fun (e, group) -> List.length group)
      |> List.max

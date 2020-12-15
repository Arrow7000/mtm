# Metomic autocomplete challenge

- Store data
	- As tree, acyclical graph

- Probably need separate tree for search result (that contains marked strings)

# Parsing

- CsvProvider?
- 

## Serialisation

- FSharp.Data.Json
	- need to ensure it can handle DUs

## Search

- Standard search with gaps, like file finders in VS Code, Sublime Text etc do
- my interpretation is that with no query, the whole tree is returned. The longer the query string is the more it narrows down the results.
	- e.g. if query is the letter 'e' then almost all paths will match, with the first 'e' in each tree path highlighted
	- but if query is dfhskfgsfgsdfgsjfgsdjfhs that will most likely narrow down results until none match
	- if query is `medhelrec` then "medical health -> health record" will match with the matching partial strings highlighted

### Search implementation

- Recursively search through tree
- at each node check if its content matches part or all of the (remaining) query

- if the whole query string has been matched, return root, with the single line of branches that lead to it, plus all of its descendants
	- Mark all of the partial strings with `<mark>` html tags
- if matches part of the query, mark the matched partial strings and carry on searching through the descendants for the remaining string
- if leaf node is reached and the full query string hasn't been matched then return nothing for this line (empty list)

- I wonder if it might be more efficient to search from the leaf nodes upwards... But I'm not sure it is, as you still have to traverse the whole tree upwards in the worst case (of not being able to match the full string)


# MTM

## Assumptions

- The endpoint should return only full matches for the query string, not fuzzy matching
- Every character in the query string needs to be present in the results, but there can be arbitrary gaps between characters from the query string
- Similarly, matches of the query string can encompass several 'generations' down, e.g. the query string `"personalextfamilyhistory"` will match the tree of `Personal Data -> External -> Medical Health -> Family Health History`
- It would be good if the backend returned the matched characters or string sections so that the frontend wouldn't have to reimplement most of the search algorithm to find these
- With that in mind the backend wraps matched characters in the HTML `mark` element; e.g. for the above search query the annotated result would be:

```html
"<mark>Personal</mark> Data" -> "<mark>Ext</mark>ernal" -> "Medical Health" -> "Health History" -> "<mark>Family</mark> <mark>H</mark>ealth H<mark>istory</mark>"
```


## What I wanted to do but didn't have time to

I spent more than 2 hours on the task because I ended up spending a lot of time on an approach for parsing the list of parent-child relationships into a tree that didn't pan out. I ended up implementing it in a less exotic, but much simpler way.

I similarly ended up spending a lot of time on traversing the hierarchy tree to create the search result, although that was mostly finetuning the approach I started with, I didn't have to change my entire approach here.

As I was already overtime I just spent a little more time implementing a web server because that was a pretty minor task. So I ended up implementing everything I wanted. 

However I would definitely have liked to refactor the project to see if certain parts of it can be cleaned up a bit; perhaps also reorganise the code a little bit.

I would also have liked to add some high level tests to ensure that the hierarchy tree parsing and searching algorithm work correctly.


## Tradeoffs

I make quite a lot of use of folds and recursive functions, which are often harder to grok than their equivalents in imperative languages. However they let you avoid for-loops, mutation and side effects which can often let subtle bugs slip in. Of course in a project of this size an imperative language would have been fine, but I find the benefits of functional programming tends to increase with project size.


## What I would have done differently

I think the solution as it stands is ok. I think I'd have just avoided certain dead ends I took in the process of implementing, e.g.trying to construct the hierarchy tree in a pure, monadic way by folding through the list of parent-child relationships and rturning a slightly modified version of the final tree at each step. That turned out to be virtually unworkable, and certainly more complex than the simpler solution I eventually came up with.

It also took me a while to figure out how to construct a search result tree with no 'dead' branches that don't contain a full match for the query string.

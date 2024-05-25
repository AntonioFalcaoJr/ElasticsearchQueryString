# Syntax

## Basic Search

**Search for a single term:**

```sh
quick
```

Searches for documents containing the word `quick`

**Search for a phrase:**

```sh
"quick brown"
```

Searches for documents containing the **exact** phrase `quick brown`

## Boolean Operators

**AND operator:**

```sh
quick AND brown
```

Searches for documents containing both the words `quick` **and** `brown`

**OR operator:**

```sh
quick OR brown
```

Searches for documents containing either the word `quick` **or** the word `brown`

**NOT operator:**

```sh
quick NOT brown
```

Searches for documents containing the word `quick` but **not** the word `brown`

**Required and Prohibited terms:**

```sh
+quick -brown
```

Searches for documents that must contain the word `quick` and **must not** contain the word `brown`

## Wildcards

**Single character wildcard:**

```sh
qu?ck
```

Searches for documents containing terms like `quick` **or** `quack` where `?` replaces a **single character**

**Multiple character wildcard:**

```sh
qu*ck
```

Searches for documents containing terms like `quick`, `quack`, **or** `question mark` where `*` replaces **zero** or **more characters**

## Proximity Searches

**Proximity search:**

```sh
"quick fox"~5
```

Searches for documents where the words `quick` **and** `fox` are within **5** words of each other

## Ranges

**Range search for numbers:**

```sh
count:[1 TO 5]
```

Searches for documents where the count is **between** `1` **and** `5`, inclusive

**Range search for dates:**

```sh
date:[2022-01-01 TO 2022-12-31]
```

Searches for documents with dates in the year 2022

**Open-ended range:**

```sh
age:>30
```

Searches for documents where the age is **greater than** `30`

## Fuzzy Searches

**Fuzzy search:**

```sh
quikc~
```

Searches for documents containing terms **similar** to `quikc`, such as `quick`, allowing for minor typos

**Fuzzy search with edit distance:**

```sh
quikc~1
```

Searches for documents containing terms similar to `quikc` with **at most one** edit, such as `quick`

## Grouping

**Grouping terms:**

```sh
(quick OR brown) AND fox
```

Searches for documents containing the word `fox` **and** either `quick` **or** `brown`

## Synonyms

**Synonyms expansion:**

```sh
ny city
```

Searches for documents containing `ny` **or** `new york` **and** the word `city`

## Minimum Should Match

**Minimum should match:**

```sh
quick brown fox~2
```

Searches for documents containing **at least two** of the terms `quick`, `brown`, **or** `fox`

## Regular Expressions

**Regular expression search:**

```sh
/qu[ia]ck/
```

Searches for documents containing terms like `quick` **or** `quack` where the character in brackets can be **either** `i` **or** `a`

## Escaping Special Characters

**Escaping special characters:**

```sh
\(1\+1\)=2
```

Searches for documents containing the **literal** text `(1+1)=2`

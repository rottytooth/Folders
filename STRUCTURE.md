# Folders — Program Structure Guide

Folders programs are encoded entirely in directory hierarchies. There is no text source code; the folder names and nesting encode the program.

## Top-level structure

Each immediate subdirectory of the root represents one statement, numbered by sort order.

## Statement types (subfolder count in statement root)

| Subfolders | Statement type |
|---|---|
| 0 | No-op |
| 1 | If |
| 2 | While |
| 3 | Print |
| 4 | Input |
| 5 | Assign |
| 6 | Call |

## Expression types (subfolder count in expression dir)

| Subfolders | Expression |
|---|---|
| 0 | Variable reference |
| 1 | Integer literal |
| 2 | String literal |
| 3 | Add |
| 4 | Subtract |
| 5 | Multiply |
| 6 | Divide |
| 7 | Equals |
| 8 | Less-than |
| 9 | Greater-than |
| 10 | And |
| 11 | Or |
| 12 | Not |

## String literal encoding

7-level nesting encodes one character (MSB first, one bit per level). Use FoldersTools to generate automatically:

```
FoldersTools LiteralBuilder string "Hello, World!"
```

## Further reading

- [Official spec](http://danieltemkin.com/Esolangs/Folders/)
- [esolangs.org/wiki/Folders](https://esolangs.org/wiki/Folders)

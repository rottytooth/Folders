# Folders
A language where the code is written with folders. Perhaps the most Windows of languages.

Hello, World!

![hellworld](https://cloud.githubusercontent.com/assets/1832142/11858142/d33b1da8-a42c-11e5-9aae-9f698d6088c1.png)

**Links**
 * intro here: http://danieltemkin.com/Esolangs/Folders/
 * and on esolangs: http://esolangs.org/wiki/Folders

## Tools

Since writing constants in Folders is a challenge (especially strings), the Folders Tools converts any literal into a set of folders:

*Usage:*
  FoldersTools LiteralBuilder type value [add_gitignore]
  
  type = char, int, float, string
  
  value = the value to convert
  
  [add_gitignore] (optional) allows for adding .gitignores to all terminal folders (necessary to check in Folders programs)

## Compiling

The Folders project itself is the compiler. Run it, providing the path to your Folders programs

*Args*:

  /s to transpile to C#
  
# to-ooxml downstream patches

This branch is the pinned DocSharp source used by
[`kvist-no/to-ooxml`](https://github.com/kvist-no/to-ooxml). It is based on
DocSharp v0.21.0 at upstream commit
`7c996f8f6934139d4a4a698963baeb59fd8d60df`.

The downstream patch is intentionally small and suitable for upstreaming:

- compare CFB paths and stream names with ordinal, case-insensitive semantics;
- retain Word OLE and VBA streams when producers vary CFB name casing;
- discover the optional PowerPoint `Pictures` stream case-insensitively;
- discover Excel `Workbook`/`Book` and VBA storage case-insensitively;
- calculate OOXML relative paths correctly when the browser-WASM virtual current
  directory is the filesystem root.

The consuming application roots DocSharp's reflection-driven record and shape
assemblies during browser-WASM trimming. That build policy lives in `to-ooxml`,
not in this fork.

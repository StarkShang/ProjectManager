# Prject Finder

Find .net project path.

## Command Line Design

### goto [alias]

Goto directory specific by argument `alias`

- The `alias` is a project name, if the current directory is a subdirectory of a solution.
  - If the `alias` is not specified, goto the root path of current solution.
  - If a unique project is macthed, goto the project root path.
  - If multiple projects are matched, list all matched projects.
  - If no project is matched, exit with error message and Exit Code 1.
- The `alias` is a directory name, if the current directory is **not** a subdirectory of a solution
  - If a unique directory in saved list is mathed, goto the directory.
  - If multiple directory in saved list is mathed, list all matched directories.
  - If no directory is matched, exit with error message and Exit Code 1.

### goto add [path] [-n | --name [name]]

Add a directory in saved list.

Arguments

`path` : specific a directory path to be saved. If the `path` is not specified, the current directory path will be saved.

Option

`-n | --name` : make an alias for saved directory. If the alias already exists in saved list, query for whether to update the record.

### goto delete alias

Delete a record in saved list by alias. The whole alias should be given.

- If a unique record is matched, delete it and display the deleted record.
- If no record is matched, just info the end user about it.

### goto list

List all save directories.


## To do list

1. 修改路径名，路径全小写，符合 Linux 规范。
2. install.sh 增加 --self-contained 参数，决定是否使用 Native 编译
# How to use

use the following command syntax:
> INIMerger **[-t|-tron]** (Output file name) (INI file with less merge prority) ... (sequence of files) ... (INI file with greatest merge priority)

File ordering is necessary: it is used to contriol which key value should be applied in conflict situation.

example:
> INIMerger merged.ini C.ini B.ini A.ini

To C.ini will be applied B.ini with section merge and key overwrite in conflict, and then will be applied A.ini.
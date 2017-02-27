// ReSharper disable InconsistentNaming

using System;


namespace FileOperationInterop {
    //Note  If this method is not called, the default value used by the operation is FOF_ALLOWUNDO | FOF_NOCONFIRMMKDIR.

    //FOF_ALLOWUNDO (0x0040)
    //    Preserve undo information, if possible.
    //    Prior to Windows Vista, operations could be undone only from the same process that performed the original operation.
    //    In Windows Vista and later systems, the scope of the undo is a user session. Any process running in the user session can undo another operation. The undo state is held in the Explorer.exe process, and as long as that process is running, it can coordinate the undo functions.
    //    If the source file parameter does not contain fully qualified path and file names, this flag is ignored.

    //FOF_FILESONLY (0x0080)
    //    Perform the operation only on files (not on folders) if a wildcard file name (*.*) is specified.

    //FOF_NOCONFIRMATION (0x0010)
    //    Respond with Yes to All for any dialog box that is displayed.

    //FOF_NOCONFIRMMKDIR (0x0200)
    //    Do not confirm the creation of a new folder if the operation requires one to be created.

    //FOF_NO_CONNECTED_ELEMENTS (0x2000)
    //    Do not move connected items as a group. Only move the specified files.

    //FOF_NOCOPYSECURITYATTRIBS (0x0800)
    //    Do not copy the security attributes of the item.

    //FOF_NOERRORUI (0x0400)
    //    Do not display a message to the user if an error occurs. If this flag is set without FOFX_EARLYFAILURE, any error is treated as if the user had chosen Ignore or Continue in a dialog box. It halts the current action, sets a flag to indicate that an action was aborted, and proceeds with the rest of the operation.

    //FOF_NORECURSION (0x1000)
    //    Only operate in the local folder. Do not operate recursively into subdirectories.

    //FOF_RENAMEONCOLLISION (0x0008)
    //    Give the item being operated on a new name in a move, copy, or rename operation if an item with the target name already exists.

    //FOF_SILENT (0x0004)
    //    Do not display a progress dialog box.

    //FOF_WANTNUKEWARNING (0x4000)
    //    Send a warning if a file or folder is being destroyed during a delete operation rather than recycled. This flag partially overrides FOF_NOCONFIRMATION.

    //FOFX_ADDUNDORECORD (0x20000000)
    //    Introduced in Windows 8. The file operation was user-invoked and should be placed on the undo stack. This flag is preferred to FOF_ALLOWUNDO.

    //FOFX_NOSKIPJUNCTIONS (0x00010000)
    //    Walk into Shell namespace junctions. By default, junctions are not entered. For more information on junctions, see Specifying a Namespace Extension's Location.

    //FOFX_PREFERHARDLINK (0x00020000)
    //    If possible, create a hard link rather than a new instance of the file in the destination.

    //FOFX_SHOWELEVATIONPROMPT (0x00040000)
    //    If an operation requires elevated rights and the FOF_NOERRORUI flag is set to disable error UI, display a UAC UI prompt nonetheless.

    //FOFX_EARLYFAILURE (0x00100000)
    //    If FOFX_EARLYFAILURE is set together with FOF_NOERRORUI, the entire set of operations is stopped upon encountering any error in any operation. This flag is valid only when FOF_NOERRORUI is set.

    //FOFX_PRESERVEFILEEXTENSIONS (0x00200000)
    //    Rename collisions in such a way as to preserve file name extensions. This flag is valid only when FOF_RENAMEONCOLLISION is also set.

    //FOFX_KEEPNEWERFILE (0x00400000)
    //    Keep the newer file or folder, based on the Date Modified property, if a collision occurs. This is done automatically with no prompt UI presented to the user.

    //FOFX_NOCOPYHOOKS (0x00800000)
    //    Do not use copy hooks.

    //FOFX_NOMINIMIZEBOX (0x01000000)
    //    Do not allow the progress dialog to be minimized.

    //FOFX_MOVEACLSACROSSVOLUMES (0x02000000)
    //    Copy the security attributes of the source item to the destination item when performing a cross-volume move operation. Without this flag, the destination item receives the security attributes of its new folder.

    //FOFX_DONTDISPLAYSOURCEPATH (0x04000000)
    //    Do not display the path of the source item in the progress dialog.

    //FOFX_DONTDISPLAYDESTPATH (0x08000000)
    //    Do not display the path of the destination item in the progress dialog.

    //FOFX_RECYCLEONDELETE (0x00080000)
    //    Introduced in Windows 8. When a file is deleted, send it to the Recycle Bin rather than permanently deleting it.

    //FOFX_REQUIREELEVATION (0x10000000)
    //    Introduced in Windows Vista SP1. The user expects a requirement for rights elevation, so do not display a dialog box asking for a confirmation of the elevation.

    //FOFX_COPYASDOWNLOAD (0x40000000)
    //    Introduced in Windows 7. Display a Downloading instead of Copying message in the progress dialog.

    //FOFX_DONTDISPLAYLOCATIONS (0x80000000)
    //    Introduced in Windows 7. Do not display the location line in the progress dialog.

    [Flags]
    internal enum FileOperationFlags : uint {
        FOF_MULTIDESTFILES = 0x0001,
        FOF_CONFIRMMOUSE = 0x0002,
        FOF_SILENT = 0x0004,  // don't create progress/report
        FOF_RENAMEONCOLLISION = 0x0008,  // Give the item being operated on a new name in a move, copy, or rename operation if an item with the target name already exists.
        FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
        FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings; Must be freed using SHFreeNameMappings
        FOF_ALLOWUNDO = 0x0040, // Preserve undo information, if possible.
        FOF_FILESONLY = 0x0080,  // on *.*, do only files
        FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
        FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
        FOF_NOERRORUI = 0x0400,  // don't put up error UI
        FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
        FOF_NORECURSION = 0x1000,  // don't recurse into directories.
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected file elements.
        FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
        FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers

        FOFX_NOSKIPJUNCTIONS = 0x00010000,  // Don't avoid binding to junctions (like Task folder, Recycle-Bin)
        FOFX_PREFERHARDLINK = 0x00020000,  // Create hard link if possible
        FOFX_SHOWELEVATIONPROMPT = 0x00040000,  // Show elevation prompts when error UI is disabled (use with FOF_NOERRORUI)
        FOFX_EARLYFAILURE = 0x00100000,  // Fail operation as soon as a single error occurs rather than trying to process other items (applies only when using FOF_NOERRORUI)
        FOFX_PRESERVEFILEEXTENSIONS = 0x00200000,  // Rename collisions preserve file extns (use with FOF_RENAMEONCOLLISION)
        FOFX_KEEPNEWERFILE = 0x00400000,  // Keep newer file on naming conflicts
        FOFX_NOCOPYHOOKS = 0x00800000,  // Don't use copy hooks
        FOFX_NOMINIMIZEBOX = 0x01000000,  // Don't allow minimizing the progress dialog
        FOFX_MOVEACLSACROSSVOLUMES = 0x02000000,  // Copy security information when performing a cross-volume move operation
        FOFX_DONTDISPLAYSOURCEPATH = 0x04000000,  // Don't display the path of source file in progress dialog
        FOFX_DONTDISPLAYDESTPATH = 0x08000000,  // Don't display the path of destination file in progress dialog
    }
}
                                                             
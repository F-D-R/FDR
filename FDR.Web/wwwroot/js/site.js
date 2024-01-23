// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SelectDrive() {
    $('#SelectedFolder').val($('#Drives').val());
    $('#SubfoldersDiv').load(`/Folder/Subfolders?folder=${encodeURIComponent($('#SelectedFolder').val())}`);
}

function SelectSubfolder() {
    $('#SelectedFolder').val($('#Subfolders').val());
    $('#SubfoldersDiv').load(`/Folder/Subfolders?folder=${encodeURIComponent($('#SelectedFolder').val())}`);
}

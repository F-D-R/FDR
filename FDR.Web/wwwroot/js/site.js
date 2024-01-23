// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function SelectDrive() {
    $('#SelectedFolder').val($('#Drives').val());
    $('#Subfolders').load(`/Folder/Subfolders?folder=${encodeURIComponent($('#SelectedFolder').val())}`);
}

function SelectSubfolder() {
    $('#SelectedFolder').val($('#Subfolders').val());
    $('#Subfolders').load(`/Folder/Subfolders?folder=${encodeURIComponent($('#SelectedFolder').val())}`);
}

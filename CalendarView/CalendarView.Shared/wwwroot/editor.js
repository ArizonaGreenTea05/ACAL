// Download file functionality
window.downloadFile = function (filename, base64Content) {
    const linkSource = `data:application/json;base64,${base64Content}`;
    const downloadLink = document.createElement("a");
    downloadLink.href = linkSource;
    downloadLink.download = filename;
    downloadLink.click();
};

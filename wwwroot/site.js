/**
 * Downloads a file from base64 content
 * @param {string} filename - The name of the file to download
 * @param {string} base64Content - Base64 encoded file content
 */
function downloadFile(filename, base64Content) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64Content;
    document.body.appendChild(link);
    
    try {
        link.click();
    } finally {
        document.body.removeChild(link);
    }
}

mergeInto(LibraryManager.library, {

    DownloadFile: function (fileName, base64Data) {
         var a = document.createElement("a"); //Create <a>
        a.href = "data:image/png;base64," + UTF8ToString(base64Data); //Image Base64 Goes here
        a.download = UTF8ToString(fileName); //File name Here
        a.style.display = 'none';
        document.body.appendChild(a);    
        a.click();    
        document.body.removeChild(a);
    },
    
    UploadFile: function (callbackMethodName) {
        var input = document.createElement("input");
        input.type = "file";
        input.accept = "image/png";
        
        input.onchange = function (event) {
            var file = event.target.files[0];
            if (file) {
                // Check if the file is a PNG or JPEG
                if(file.type === "image/png" || file.type === "image/jpeg") {
                    const reader = new FileReader();
                    reader.onload = function(event) {
                        const img = new Image();
                        img.onload = function() {
                            const canvas = document.createElement('canvas');
                            canvas.width = img.width;
                            canvas.height = img.height;
                            const ctx = canvas.getContext('2d');
                            ctx.drawImage(img, 0, 0);
                            // Convert to PNG format
                            const base64String = canvas.toDataURL('image/png');
                            {{{ makeDynCall('vi', 'callbackMethodName') }}} (stringToNewUTF8(base64String));
                        };
                        img.src = event.target.result;
                    };
                    reader.readAsDataURL(file);
                }
            }
        };
        input.click();
    }
});

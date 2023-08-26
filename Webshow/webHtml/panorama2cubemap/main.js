const canvas = document.createElement('canvas');
const ctx = canvas.getContext('2d');

class RadioInput {
    constructor(name, onChange) {
        this.inputs = document.querySelectorAll(`input[name=${name}]`);
        for (let input of this.inputs) {
            input.addEventListener('change', onChange);
        }
    }

    get value() {
        for (let input of this.inputs) {
            if (input.checked) {
                return input.value;
            }
        }
    }
}

class Input {
    constructor(id, onChange) {
        this.input = document.getElementById(id);
        this.input.addEventListener('change', onChange);
        this.valueAttrib = this.input.type === 'checkbox' ? 'checked' : 'value';
    }

    get value() {
        return this.input[this.valueAttrib];
    }
}

class CubeFace {
    constructor(faceName) {
        this.faceName = faceName;

        this.anchor = document.createElement('a');
        this.anchor.style.position = 'absolute';
        this.anchor.title = faceName;

        this.img = document.createElement('img');
        this.img.style.filter = 'blur(4px)';

        this.anchor.appendChild(this.img);
    }

    setPreview(url, x, y) {
        this.img.src = url;
        this.anchor.style.left = `${x}px`;
        this.anchor.style.top = `${y}px`;
    }

    setDownload(url, fileExtension) {
        this.anchor.href = url;
        this.anchor.download = `${this.faceName}.${fileExtension}`;
        this.img.style.filter = '';
    }
}

function removeChildren(node) {
    while (node.firstChild) {
        node.removeChild(node.firstChild);
    }
}

const mimeType = {
    'jpg': 'image/jpeg',
    'png': 'image/png'
};

function getDataURL(imgData, extension) {
    canvas.width = imgData.width;
    canvas.height = imgData.height;
    ctx.putImageData(imgData, 0, 0);
    return new Promise(resolve => {
        canvas.toBlob(blob => resolve(URL.createObjectURL(blob)), mimeType[extension], 0.92);
    });
}

const dom = {
    imageInput: document.getElementById('imageInput'),
    faces: document.getElementById('faces'),
    generating: document.getElementById('generating')
};

dom.imageInput.addEventListener('change', loadImage);

const settings = {
    cubeRotation: new Input('cubeRotation', loadImage),
    interpolation: new RadioInput('interpolation', loadImage),
    format: new RadioInput('format', loadImage),
};

const facePositions = {
    pz: { x: 1, y: 1 },
    nz: { x: 3, y: 1 },
    px: { x: 2, y: 1 },
    nx: { x: 0, y: 1 },
    py: { x: 1, y: 0 },
    ny: { x: 1, y: 2 }
};

function loadImage() {
    const file = dom.imageInput.files[0];

    if (!file) {
        return;
    }

    const img = new Image();

    img.src = URL.createObjectURL(file);

    img.addEventListener('load', () => {
        const { width, height } = img;
        canvas.width = width;
        canvas.height = height;
        ctx.drawImage(img, 0, 0);
        const data = ctx.getImageData(0, 0, width, height);

        processImage(data);
    });
}

let finished = 0;
let workers = [];

function processImage(data) {
    removeChildren(dom.faces);
    dom.generating.style.visibility = 'visible';

    for (let worker of workers) {
        worker.terminate();
    }

    for (let [faceName, position] of Object.entries(facePositions)) {
        renderFace(data, faceName, position);
    }
}

function renderFace(data, faceName, position) {
    const face = new CubeFace(faceName);
    dom.faces.appendChild(face.anchor);

    const options = {
        data: data,
        face: faceName,
        rotation: Math.PI * settings.cubeRotation.value / 180,
        interpolation: settings.interpolation.value,
    };

    const worker = new Worker('convert.js');

    const setDownload = ({ data: imageData }) => {
        const extension = settings.format.value;

        getDataURL(imageData, extension)
            .then(url => face.setDownload(url, extension));

        finished++;

        if (finished === 6) {
            dom.generating.style.visibility = 'hidden';
            finished = 0;
            workers = [];
        }
    };

    const setPreview = ({ data: imageData }) => {
        const x = imageData.width * position.x;
        const y = imageData.height * position.y;

        getDataURL(imageData, 'jpg')
            .then(url => face.setPreview(url, x, y));

        worker.onmessage = setDownload;
        worker.postMessage(options);
    };

    worker.onmessage = setPreview;
    worker.postMessage(Object.assign({}, options, {
        maxWidth: 200,
        interpolation: 'linear',
    }));

    workers.push(worker);
}

var writeIntoLh = function () {
    function toDataURL(src, titleOfImage, outputFormat) {
        var img = new Image();
        img.crossOrigin = 'Anonymous';
        img.onload = function () {
            var canvas = document.createElement('CANVAS');
            var ctx = canvas.getContext('2d');
            var dataURL;
            var widthOfImage = 512;
            canvas.height = widthOfImage;
            canvas.width = widthOfImage;
            ctx.drawImage(this, 0, 0, widthOfImage, widthOfImage);
            dataURL = canvas.toDataURL(outputFormat);
            window.localStorage[titleOfImage] = dataURL;
            // callback(dataURL);
        };
        img.src = src;
        if (img.complete || img.complete === undefined) {
            img.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
            img.src = src;
        }
    };

    //var cubeValue = {};
    for (var i = 0; i < 6; i++) {
        var operateIndex = i + 0;
        var fss = document.getElementById('faces');
        var titleOfImage = fss.children[operateIndex].title;

        toDataURL(
            fss.children[operateIndex].href,
            titleOfImage
        )
    }
    //  window.localStorage.cubeValue = JSON.stringify(cubeValue);
}

var writeToServer = function () {
    //var   window.localStorage[]

    var crossName = window.localStorage['crossName'];
    if (crossName) { }
    else {
        alert('没有路口数据');
        return;
    }
    var faces = document.getElementById('faces');
    for (var i = 0; i < faces.children.length; i++) {
        var face = faces.children[i];
        //var href = face.href;
        //var title = face.title;
        var xhr = new XMLHttpRequest();
        xhr.open('GET', face.href, true);
        xhr.responseType = 'blob';
        xhr.onload = function (e) {
            if (this.status == 200) {
                var myBlob = this.response;
                //console.log('myBlob:', myBlob);
                //console.log('myBlob:' + title, e);
                //console.log('myBlob:' + title, this);
                var faces = document.getElementById('faces');
                var title = 'null';
                for (var j = 0; j < faces.children.length; j++) {
                    var face = faces.children[j];
                    if (face.href == this.responseURL) {
                        title = face.title;
                        var fd = new FormData();
                        fd.append('crossName', crossName);
                        fd.append('fname', title);

                        fd.append('data', myBlob);
                        $.ajax({
                            type: 'POST',
                            url: 'https://www.nyrq123.com/websockettaiyuaneditor/upload',
                            data: fd,
                            processData: false,
                            contentType: false
                        }).done(function (data) {
                            console.log(data);
                        });
                        continue;
                    }
                }
                console.log('myBlob:' + title, myBlob);
            }
        };
        xhr.send();
    }

}

//var messageSign = '';

var writeToServerAndBindAddr = function () {
    var msg = '';
    if (window.localStorage['fpBGMsg'] == undefined) {
        let date = new Date();
        let year = date.getFullYear();
        let month = date.getMonth() + 1;
        let day = date.getDate();

        if (month < 10) {
            month = "0" + month;
        }

        if (day < 10) {
            day = "0" + day;
        }

        let formattedDate = `${year}${month}${day}`;
        msg = prompt('输入日期，.如' + formattedDate, formattedDate);
        window.localStorage['fpBGMsg'] = msg;
    }
    else {
        msg = window.localStorage['fpBGMsg'];
    }
    var addr = '';
    if (window.localStorage['fpBGAddr'] == undefined) {
        addr = prompt('输入B地址');
        window.localStorage['fpBGAddr'] = addr;
    }
    else {
        addr = window.localStorage['fpBGAddr'];
    }

    var messageSign = '';
    if (window.localStorage['fpBGmessageSign'] == undefined) {
        messageSign = prompt('输入签名');
        window.localStorage['fpBGmessageSign'] = messageSign;
    }
    else {
        messageSign = window.localStorage['fpBGmessageSign'];
    }




    //var sign = '';

    var successCount = 0;
    var fpCode = window.prompt('输入对应的地址编码', '');
    window.localStorage['fpCode'] = fpCode;
    //if (fpName) { }
    //else {
    //    alert('没有路口数据');
    //    return;
    //}
    var faces = document.getElementById('faces');
    for (var i = 0; i < faces.children.length; i++) {
        var face = faces.children[i];
        //var href = face.href;
        //var title = face.title;
        var xhr = new XMLHttpRequest();
        xhr.open('GET', face.href, true);
        xhr.responseType = 'blob';
        xhr.onload = function (e) {
            if (this.status == 200) {
                var myBlob = this.response;
                //console.log('myBlob:', myBlob);
                //console.log('myBlob:' + title, e);
                //console.log('myBlob:' + title, this);
                var faces = document.getElementById('faces');
                var title = 'null';
                for (var j = 0; j < faces.children.length; j++) {
                    var face = faces.children[j];
                    if (face.href == this.responseURL) {
                        title = face.title;
                        var fd = new FormData();
                        fd.append('messageSign', messageSign);
                        fd.append('msg', msg);
                        fd.append('addr', addr);
                        fd.append('fpCode', fpCode);
                        fd.append('fname', title);
                        fd.append('command', 'pass');

                        fd.append('data', myBlob);

                        var url = 'http://127.0.0.1:21001/fpupload' + '?face=' + face + '&fpCode=' + fpCode + '&t=' + Date.now();
                        //if (objMain.debug)
                        //url = 'http://127.0.0.1:21001/fpupload' + '?face=' + face + '&fpCode=' + fpCode + '&t=' + Date.now();
                        //else
                        url = 'https://www.nyrq123.com/websocket' + window.location.pathname.split('/')[1] + 'editor/fpupload' + '?face=' + face + '&fpCode=' + fpCode + '&t=' + Date.now();
                        // url: 'https://www.nyrq123.com/websockettaiyuaneditor/fpupload',
                        //var url = 'http://127.0.0.1:21001/fpupload' + '?face=' + face + '&fpCode=' + fpCode + '&t=' + Date.now();
                        $.ajax({
                            type: 'POST',
                            url: url,
                            data: fd,
                            processData: false,
                            contentType: false
                        }).done(function (data) {
                            successCount++;
                            console.log('data', data);
                            if (successCount == 6) {
                                var fd = new FormData();
                                fd.append('messageSign', messageSign);
                                fd.append('msg', msg);
                                fd.append('addr', addr);
                                fd.append('fpCode', fpCode);
                                fd.append('command', 'save');
                                $.ajax({
                                    type: 'POST',
                                    url: url,
                                    data: fd,
                                    processData: false,
                                    contentType: false
                                }).done(function (data) {
                                    alert('存储成功');
                                    successCount++;
                                    console.log('data', data);
                                });
                            }
                            console.log(data);
                        });
                        continue;
                    }
                }
                console.log('myBlob:' + title, myBlob);
            }
        };
        xhr.send();
    }
}

var clearLoginInfo = function () {
    delete window.localStorage['fpBGMsg'];
    delete window.localStorage['fpBGAddr'];
    delete window.localStorage['fpBGmessageSign'];
}
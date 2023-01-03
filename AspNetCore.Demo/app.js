import mirrorsharp from 'mirrorsharp';

const params = window.location.search.replace(/^\?/, '').split('&').reduce(function (o, item) {
    const parts = item.split('=');
    o[parts[0]] = parts[1];
    return o;
}, {});
const language = (params['language'] || 'CSharp').replace('Sharp', '#');
const mode = params['mode'] || 'regular';

const code = `using System;

class C {
    const int C2 = 5;
    string f;
    string P { get; set; }
    event EventHandler e;
    event EventHandler E { add {} remove {} }

    C() {
    }

    void M(int p) {
        var l = p;
    }
}

class G<T> {
}`.replace(/(\r\n|\r|\n)/g, '\r\n'); // Parcel changes newlines to LF

if (language === 'F#') {
    code = '[<EntryPoint>]\r\nlet main argv = \r\n    0';
}
else if (mode === 'script') {
    code = 'var messages = Context.Messages;';
}
else if (language === 'IL') {
    code = '.class private auto ansi \'<Module>\'\r\n{\r\n}';
}

mirrorsharp(document.getElementById('editor-container'), {
    serviceUrl: window.location.href.replace(/^http(s?:\/\/[^/]+).*$/i, 'ws$1/mirrorsharp'),
    selfDebugEnabled: true,
    language,
    initialText: code,
    initialServerOptions: (mode !== 'regular' ? { 'x-mode': mode } : {})
});
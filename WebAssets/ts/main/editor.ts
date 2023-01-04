import { EditorView } from '@codemirror/view';
//import 'codemirror/mode/clike/clike';
//import 'codemirror-addon-infotip';
//import 'codemirror-addon-lint-fix';
import {
    Message,
    // ChangeData,
    SlowUpdateMessage,
    DiagnosticSeverity,
    ServerOptions,
    // SpanData,
    Language,
    DEFAULT_LANGUAGE
} from '../interfaces/protocol';
import type { SlowUpdateOptions } from '../interfaces/slow-update';
import type { Connection } from './connection';
// import type { SelfDebug } from './self-debug';
import { createState } from './codemirror/create-state';
// import { renderInfotip } from './render-infotip';
// import { Hinter } from './hinter';
// import { SignatureTip } from './signature-tip';
import { addEvents } from '../helpers/add-events';
import type { Session } from './session';

/*const indexKey = '$mirrorsharp-index';
interface PositionWithIndex extends CodeMirror.Position {
    [indexKey]: number;
}

interface DiagnosticAnnotation extends CodeMirror.Annotation {
    readonly diagnostic: DiagnosticData;
}

interface AnnotationFixWithId extends CodeMirror.AnnotationFix {
    readonly id: number;
}*/

interface EditorOptions<TExtensionServerOptions, TSlowUpdateExtensionData> {
    readonly language?: Language;
    readonly initialText?: string;
    readonly initialCursorOffset?: number;

    readonly on?: {
        readonly textChange?: (getText: () => string) => void;
        readonly connectionChange?: {
            (event: 'open', e: Event): void;
            (event: 'error', e: ErrorEvent): void;
            (event: 'close', e: CloseEvent): void;
        };
        readonly serverError?: (message: string) => void;
    } & SlowUpdateOptions<TSlowUpdateExtensionData>;

    readonly initialServerOptions?: TExtensionServerOptions;
}

// const languageModes = {
//     'C#': 'text/x-csharp',
//     'Visual Basic': 'text/x-vb',
//     'F#': 'text/x-fsharp',
//     'IL': 'text/x-cil',
//     'PHP': 'application/x-httpd-php'
// } as const;

// const lineSeparator = '\r\n';

export class Editor<TExtensionServerOptions, TSlowUpdateExtensionData> {
    readonly #connection: Connection<TExtensionServerOptions, TSlowUpdateExtensionData>;
    readonly #session: Session<TExtensionServerOptions>;
    // readonly #selfDebug: SelfDebug|null;
    readonly #options: EditorOptions<TExtensionServerOptions, TSlowUpdateExtensionData>;

    // readonly #cm: CodeMirror.EditorFromTextArea;
    readonly #wrapper: HTMLElement;
    readonly #cmView: EditorView;
    // readonly #hinter: Hinter<TExtensionServerOptions, TSlowUpdateExtensionData>;
    // readonly #signatureTip: InstanceType<typeof SignatureTip>;

    // readonly #keyMap: CodeMirror.KeyMap;
    // readonly #removeCodeMirrorEvents: () => void;
    readonly #removeConnectionEvents: () => void;

    #language: Language;
    #serverOptions: ServerOptions & TExtensionServerOptions;
    #pendingServerOptions: (ServerOptions & TExtensionServerOptions) | null | undefined;

    #lintingSuspended = true;
    #hadChangesSinceLastLinting = false;
    // #capturedUpdateLinting: CodeMirror.UpdateLintingCallback | null | undefined;

    // #changePending = false;
    // #changeReason: string|null = null;
    // #changesAreFromServer = false;

    constructor(
        container: HTMLElement,
        connection: Connection<TExtensionServerOptions, TSlowUpdateExtensionData>,
        session: Session<TExtensionServerOptions>,
        // selfDebug: SelfDebug|null,
        options: EditorOptions<TExtensionServerOptions, TSlowUpdateExtensionData>
    ) {
        this.#connection = connection;
        this.#session = session;
        // this.#selfDebug = selfDebug;

        options = {
            language: DEFAULT_LANGUAGE,
            ...options,
            on: {
                slowUpdateWait:   () => ({}),
                slowUpdateResult: () => ({}),
                textChange:       () => ({}),
                connectionChange: () => ({}),
                serverError:      (message: string) => { throw new Error(message); },
                ...options.on
            }
        };
        this.#options = options;

        // const cmOptions = {
        //     gutters: [],
        //     indentUnit: 4,
        //     ...options.forCodeMirror,
        //     lineSeparator,
        //     // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        //     mode: languageModes[options.language!],
        //     lint: { async: true, getAnnotations: this.#lintGetAnnotations, hasGutters: true },
        //     lintFix: { getFixes: this.#getLintFixes },
        //     infotip: { async: true, delay: 500, getInfo: this.#infotipGetInfo, render: renderInfotip }
        // } as CodeMirror.EditorConfiguration & { lineSeparator: string };

        // // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        // cmOptions.gutters!.push('CodeMirror-lint-markers');

        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        this.#language = options.language!;
        this.#serverOptions = {
            ...(options.initialServerOptions ?? {}),
            language: this.#language
        } as ServerOptions & TExtensionServerOptions;

        this.#session.setOptions(this.#serverOptions);

        // const cmSource = (function getCodeMirror() {
        //     const next = textarea.nextSibling as { CodeMirror?: CodeMirror.EditorFromTextArea };
        //     if (next?.CodeMirror) {
        //         const existing = next.CodeMirror;
        //         for (const key in cmOptions) {
        //             // eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/no-unsafe-call
        //             existing.setOption(key as any, cmOptions[key as keyof typeof cmOptions]);
        //         }
        //         return { cm: existing, existing: true };
        //     }

        //     return { cm: CodeMirror.fromTextArea(textarea, cmOptions) };
        // })();

        // this.#cm = cmSource.cm;
        // this.#keyMap = {
        //     /* eslint-disable object-shorthand */
        //     'Tab': () => {
        //         if (this.#cm.somethingSelected()) {
        //             this.#cm.execCommand('indentMore');
        //             return;
        //         }
        //         this.#cm.replaceSelection('    ', 'end');
        //     },
        //     'Shift-Tab': 'indentLess',
        //     // eslint-disable-next-line @typescript-eslint/no-floating-promises
        //     'Ctrl-Space': () => { connection.sendCompletionState('force'); },
        //     // eslint-disable-next-line @typescript-eslint/no-floating-promises
        //     'Shift-Ctrl-Space': () => { connection.sendSignatureHelpState('force'); },
        //     'Ctrl-.': 'lintFixShow',
        //     'Shift-Ctrl-Y': selfDebug ? () => {
        //         // eslint-disable-next-line @typescript-eslint/no-floating-promises
        //         selfDebug.requestData(connection);
        //     } : false
        //     /* eslint-enable object-shorthand */
        // };
        // this.#cm.addKeyMap(this.#keyMap);
        // // see https://github.com/codemirror/CodeMirror/blob/dbaf6a94f1ae50d387fa77893cf6b886988c2147/addon/lint/lint.js#L133
        // // ensures that next 'id' will be -1 whether a change happened or not
        // // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
        // this.#cm.state.lint.waitingFor = -2;
        // if (!cmSource.existing)
        //     this.setText(textarea.value);

        this.#wrapper = document.createElement('div');
        this.#wrapper.classList.add('mirrorsharp');
        container.appendChild(this.#wrapper);
        this.#cmView = new EditorView({
            state: createState(this.#connection, this.#session, {
                initialText: options.initialText,
                initialCursorOffset: options.initialCursorOffset
            })
        });

        // if (selfDebug)
        //     selfDebug.watchEditor(this.getText, this.#getCursorIndex);

        this.#wrapper.appendChild(this.#cmView.dom);
        // const cmWrapper = this.#cm.getWrapperElement();
        // cmWrapper.classList.add('mirrorsharp', 'mirrorsharp-theme');

        // this.#hinter = new Hinter(/*this.#cm, connection*/);
        // this.#signatureTip = new SignatureTip(/* this.#cm */);
        this.#removeConnectionEvents = addEvents(connection, {
            open: this.#onConnectionOpen,
            message: this.#onConnectionMessage,
            error: this.#onConnectionCloseOrError,
            close: this.#onConnectionCloseOrError
        });

        // this.#removeCodeMirrorEvents = addEvents(this.#cm, {
        //     beforeChange: this.#onCodeMirrorBeforeChange,
        //     cursorActivity: this.#onCodeMirrorCursorActivity,
        //     changes: this.#onCodeMirrorChanges
        // });
    }

    #onConnectionOpen = (e: Event) => {
        this.#hideConnectionLoss();
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        this.#options.on!.connectionChange!('open', e);
    };

    #onConnectionMessage = (message: Message<TExtensionServerOptions, TSlowUpdateExtensionData>) => {
        switch (message.type) {
            case 'changes':
                //this.#receiveServerChanges(message.changes, message.reason);
                break;

            case 'completions':
                // this.#hinter.start(message.completions, message.span, {
                //     commitChars: message.commitChars,
                //     suggestion: message.suggestion
                // });
                break;

            case 'completionInfo':
                // this.#hinter.showTip(message.index, message.parts);
                break;

            case 'signatures':
                // this.#signatureTip.update(message);
                break;

            case 'infotip':
                if (!message.sections) {
                    // this.#cm.infotipUpdate(null);
                    return;
                }
                // this.#cm.infotipUpdate({
                //     data: message,
                //     range: this.#spanToRange(message.span)
                // });
                break;

            case 'slowUpdate':
                this.#showSlowUpdate(message);
                break;

            case 'optionsEcho':
                // this.#receiveServerOptions(message.options);
                break;

            case 'self:debug':
                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                // this.#selfDebug!.displayData(message);
                break;

            case 'error':
                // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                this.#options.on!.serverError!(message.message);
                break;

            default:
                throw new Error('Unknown message type "' + message.type);
        }
    };

    #onConnectionCloseOrError = (e: CloseEvent|ErrorEvent) => {
        this.#lintingSuspended = true;
        this.#showConnectionLoss();
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        const connectionChange = this.#options.on!.connectionChange!;
        if (e instanceof CloseEvent) {
            connectionChange('close', e);
        }
        else {
            connectionChange('error', e);
        }
    };

    // #onCodeMirrorBeforeChange = (_: CodeMirror.Editor, change: CodeMirror.EditorChange) => {
    //     (change.from)[indexKey] = this.#cm.indexFromPos(change.from);
    //     (change.to)[indexKey] = this.#cm.indexFromPos(change.to);
    //     this.#changePending = true;
    // };

    // #onCodeMirrorCursorActivity = () => {
    //     if (this.#changePending)
    //         return;
    //     // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //     this.#connection.sendMoveCursor(this.#getCursorIndex());
    // };

    // #onCodeMirrorChanges = (_: CodeMirror.Editor, changes: ReadonlyArray<CodeMirror.EditorChange>) => {
    //     this.#hadChangesSinceLastLinting = true;
    //     this.#changePending = false;
    //     const cursorIndex = this.#getCursorIndex();
    //     changes = this.#mergeChanges(changes);
    //     for (let i = 0; i < changes.length; i++) {
    //         const change = changes[i];
    //         const start = (change.from)[indexKey];
    //         const length = (change.to)[indexKey] - start;
    //         const text = change.text.join(lineSeparator);
    //         if (cursorIndex === start + 1 && text.length === 1 && !this.#changesAreFromServer) {
    //             if (length > 0) {
    //                 // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //                 this.#connection.sendReplaceText(start, length, '', cursorIndex - 1);
    //             }
    //             // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //             this.#connection.sendTypeChar(text);
    //         }
    //         else {
    //             // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //             this.#connection.sendReplaceText(start, length, text, cursorIndex, this.#changeReason);
    //         }
    //     }
    //     // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    //     this.#options.on!.textChange!(this.#getText);
    // };

    // #mergeChanges = (changes: ReadonlyArray<CodeMirror.EditorChange>) => {
    //     if (changes.length < 2)
    //         return changes;

    //     const canBeMerged = (first: CodeMirror.EditorChange|null, second: CodeMirror.EditorChange|null) => {
    //         return first && second
    //             && first.origin === 'undo'
    //             && second.origin === 'undo'
    //             && first.to.line === second.from.line
    //             && first.text.length === 1
    //             && second.text.length === 1
    //             && second.from.ch === second.to.ch
    //             && (first.to.ch + first.text[0].length) === second.from.ch;
    //     };

    //     const results = [];
    //     let before: CodeMirror.EditorChange|null = null;
    //     for (const change of changes) {
    //         if (canBeMerged(before, change)) {
    //             before = {
    //                 /* eslint-disable @typescript-eslint/no-non-null-assertion */
    //                 from: before.from,
    //                 to: before.to,
    //                 text: [before.text[0] + change.text[0]],
    //                 origin: change.origin
    //                 /* eslint-enable @typescript-eslint/no-non-null-assertion */
    //             };
    //         }
    //         else {
    //             if (before)
    //                 results.push(before);
    //             before = change;
    //         }
    //     }
    //     // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    //     results.push(before);
    //     return results;
    // };

    // #lintGetAnnotations = (_: string, updateLinting: CodeMirror.UpdateLintingCallback) => {
    //     if (!this.#capturedUpdateLinting) {
    //         this.#capturedUpdateLinting = function(this: ThisParameterType<CodeMirror.UpdateLintingCallback>, ...args: Parameters<CodeMirror.UpdateLintingCallback>) {
    //             const [cm] = args;
    //             // see https://github.com/codemirror/CodeMirror/blob/dbaf6a94f1ae50d387fa77893cf6b886988c2147/addon/lint/lint.js#L133
    //             // ensures that next 'id' will always match 'waitingFor'
    //             (cm.state as { lint: { waitingFor: number } }).lint.waitingFor = -1;
    //             updateLinting.apply(this, args);
    //         };
    //     }
    //     // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //     this.#requestSlowUpdate();
    // };

    // #receiveServerChanges = (changes: ReadonlyArray<ChangeData>, reason: string|null) => {
    //     this.#changesAreFromServer = true;
    //     this.#changeReason = reason ?? 'server';
    //     /*this.#cm.operation(() => {
    //         let offset = 0;
    //         for (const change of changes) {
    //             const from = this.#cm.posFromIndex(change.start + offset);
    //             const to = change.length > 0 ? this.#cm.posFromIndex(change.start + offset + change.length) : from;
    //             this.#cm.replaceRange(change.text, from, to, '+server');
    //             offset += change.text.length - change.length;
    //         }
    //     });*/
    //     this.#changeReason = null;
    //     this.#changesAreFromServer = false;
    // };

    // #getLintFixes = (cm: CodeMirror.Editor, line: number, annotations: ReadonlyArray<CodeMirror.Annotation>) => {
    //     const requestApplyFix = (cm: CodeMirror.Editor, line: number, fix: AnnotationFixWithId) => {
    //         // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //         this.#connection.sendApplyDiagnosticAction(fix.id);
    //     };

    //     const fixes: Array<CodeMirror.AnnotationFix> = [];
    //     for (const annotation of annotations) {
    //         const diagnostic = (annotation).diagnostic;
    //         if (!diagnostic.actions)
    //             continue;
    //         for (const action of diagnostic.actions) {
    //             fixes.push({
    //                 text: action.title,
    //                 apply: requestApplyFix,
    //                 id: action.id
    //             } as AnnotationFixWithId);
    //         }
    //     }
    //     return fixes;
    // };

    // #infotipGetInfo = (cm: CodeMirror.Editor, position: CodeMirror.Position) => {
    //     // eslint-disable-next-line @typescript-eslint/no-floating-promises
    //     this.#connection.sendRequestInfoTip(cm.indexFromPos(position));
    // };

    // #requestSlowUpdate = (force?: boolean) => {
    //     if (this.#lintingSuspended || !(this.#hadChangesSinceLastLinting || force))
    //         return null;
    //     this.#hadChangesSinceLastLinting = false;
    //     // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    //     this.#options.on!.slowUpdateWait!();
    //     return this.#connection.sendSlowUpdate();
    // };

    #showSlowUpdate = (update: SlowUpdateMessage<TSlowUpdateExtensionData>) => {
        // const annotations: Array<DiagnosticAnnotation> = [];

        // Higher severities must go last -- CodeMirror uses last one for the icon.
        // Unless one is error, in which case it's always error -- but still makes
        // sense to handle this consistently.
        const priorityBySeverity = { hidden: 0, info: 1, warning: 2, error: 3 };
        const diagnostics = update.diagnostics.slice(0);
        diagnostics.sort((a, b) => {
            const aOrder = priorityBySeverity[a.severity];
            const bOrder = priorityBySeverity[b.severity];
            return aOrder !== bOrder ? (aOrder > bOrder ? 1 : -1) : 0;
        });

        for (const diagnostic of diagnostics) {
            let severity: DiagnosticSeverity|'unnecessary' = diagnostic.severity;
            const isUnnecessary = diagnostic.tags.includes('unnecessary');
            if (severity === 'hidden' && !isUnnecessary)
                continue;

            if (isUnnecessary && (severity === 'hidden' || severity === 'info'))
                severity = 'unnecessary';

            // const range = this.#spanToRange(diagnostic.span);
            // annotations.push({
            //     severity,
            //     message: diagnostic.message,
            //     from: range.from,
            //     to: range.to,
            //     diagnostic
            // });
        }
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        // this.#capturedUpdateLinting(this.#cm, annotations);
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        this.#options.on!.slowUpdateResult!({
            diagnostics: update.diagnostics,
            x: update.x
        });
    };

    #connectionLossElement: HTMLDivElement|undefined;

    #showConnectionLoss = () => {
        if (!this.#connectionLossElement) {
            const connectionLossElement = document.createElement('div');
            connectionLossElement.setAttribute('class', 'mirrorsharp-connection-issue');
            connectionLossElement.innerText = 'Server connection lost, reconnecting…';
            this.#wrapper.appendChild(connectionLossElement);
            this.#connectionLossElement = connectionLossElement;
        }

        this.#wrapper.classList.add('mirrorsharp-connection-has-issue');
    };

    #hideConnectionLoss = () => {
        this.#wrapper.classList.remove('mirrorsharp-connection-has-issue');
    };

    // #sendServerOptions = async (value: ServerOptions | Partial<TExtensionServerOptions> | Partial<ServerOptions & TExtensionServerOptions>) => {
    //     this.#pendingServerOptions = { ...this.#serverOptions, ...value };
    //     await this.#connection.sendSetOptions(value);
    //     await this.#requestSlowUpdate(true);
    // };

    // #receiveServerOptions = (value: ServerOptions&TExtensionServerOptions) => {
    //     this.#pendingServerOptions = null;
    //     this.#serverOptions = { ...this.#serverOptions, ...value };
    //     // TODO: understand later
    //     // eslint-disable-next-line no-undefined
    //     if (value.language !== undefined && value.language !== this.#language) {
    //         this.#language = value.language;
    //         // this.#cm.setOption('mode', languageModes[this.#language]);
    //     }
    // };

    // #spanToRange = (span: SpanData) => {
    //     return {
    //         from: this.#cm.posFromIndex(span.start),
    //         to: this.#cm.posFromIndex(span.start + span.length)
    //     };
    // };

    // getCodeMirror() {
    //     return this.#cm;
    // }

    // setText(text: string) {
    //     this.#cm.setValue(text.replace(/(\r\n|\r|\n)/g, '\r\n'));
    // }

    getCodeMirrorView() {
        return this.#cmView;
    }

    getText() {
        return this.#cmView.state.sliceDoc();
    }

    getCursorOffset() {
        return this.#cmView.state.selection.main.from;
    }

    getLanguage() {
        return this.#language;
    }

    setLanguage(value: Language) {
        this.#session.setOptions(
            ({ language: value } satisfies Partial<ServerOptions>) as Partial<ServerOptions> & Partial<TExtensionServerOptions>
        );
    }

    setServerOptions(value: TExtensionServerOptions) {
        this.#session.setOptions(value as Partial<TExtensionServerOptions>);
    }

    destroy(destroyOptions: { readonly keepCodeMirror?: boolean } = {}) {
        // this.#cm.save();
        this.#removeConnectionEvents();
        if (!destroyOptions.keepCodeMirror) {
            // this.#cm.toTextArea();
            return;
        }
        // this.#cm.removeKeyMap(this.#keyMap);
        // this.#removeCodeMirrorEvents();
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        // this.#cm.setOption('lint', null!);
        // this.#cm.setOption('lintFix', null);
        // TODO: fix in infotip
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        // this.#cm.setOption('infotip', null!);
    }
}
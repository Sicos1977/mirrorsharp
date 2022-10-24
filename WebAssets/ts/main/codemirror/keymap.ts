
import { defaultKeymap, historyKeymap, indentMore, indentLess } from '@codemirror/commands';
import { keymap, ViewPlugin } from '@codemirror/view';
import { defineEffectField } from '../../helpers/define-effect-field';

const [tabTrapped, dispatchTabTrappedChanged] = defineEffectField<boolean>(true);
const tabTrapPlugin = ViewPlugin.define(() => ({
    update({ view, focusChanged }) {
        if (focusChanged && view.hasFocus)
            dispatchTabTrappedChanged(view, true);
    }
}));

export default [tabTrapped, tabTrapPlugin, keymap.of([
    ...defaultKeymap,
    ...historyKeymap,
    { key: 'Tab', run: view => view.state.field(tabTrapped) ? indentMore(view) : false },
    { key: 'Shift-Tab', run: view => view.state.field(tabTrapped) ? indentLess(view) : false },
    { key: 'Escape', run: view => {
        dispatchTabTrappedChanged(view, false);
        return true;
    } }
])];
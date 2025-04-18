import { Routes } from '@angular/router';
import { EditorPage } from './editor.page';
import { XmpTab } from './tabs/xmp/xmp.tab';
import { CiiTab } from './tabs/cii/cii.tab';
import { AttachmentsTab } from './tabs/attachments/attachments.tab';
import { SettingsTab } from './tabs/settings/settings.tab';

export const routes: Routes = [
  {
    path: '',
    component: EditorPage,
    children: [
      {
        path: 'xmp',
        component: XmpTab,
      },
      {
        path: 'cii',
        component: CiiTab,
      },
      {
        path: 'attachments',
        component: AttachmentsTab,
      },
      {
        path: 'settings',
        component: SettingsTab,
      },
      {
        path: '**',
        redirectTo: 'cii',
      },
    ],
  },
];

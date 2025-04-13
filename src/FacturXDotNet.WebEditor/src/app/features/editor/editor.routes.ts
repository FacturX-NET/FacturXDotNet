import { Routes } from '@angular/router';
import { EditorPage } from './editor.page';
import { XmpTab } from './tabs/xmp/xmp.tab';
import { CiiTab } from './tabs/cii/cii.tab';
import { AttachmentsTab } from './tabs/attachments/attachments.tab';

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
        path: '**',
        redirectTo: 'cii',
      },
    ],
  },
];

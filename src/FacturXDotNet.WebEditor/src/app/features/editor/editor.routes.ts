import { Routes } from '@angular/router';
import { EditorPage } from './editor.page';
import { XmpTab } from './tabs/xmp/xmp.tab';
import { CiiTab } from './tabs/cii/cii.tab';
import { AttachmentsTab } from './tabs/attachments/attachments.tab';
import { EditorSettingsTab } from './tabs/settings/editor-settings.tab';
import { EditorSettingsGeneralTab } from './tabs/settings/tabs/editor-settings-general/editor-settings-general.tab';
import { EditorSettingsPdfProfilesTab } from './tabs/settings/tabs/editor-settings-pdf-profiles/editor-settings-pdf-profiles.tab';
import { EditorSettingsPdfProfileCreateTab } from './tabs/settings/tabs/editor-settings-pdf-profiles/editor-settings-pdf-profile-create.tab';
import { EditorSettingsPdfProfileEditTab } from './tabs/settings/tabs/editor-settings-pdf-profiles/editor-settings-pdf-profile-edit.tab';

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
        component: EditorSettingsTab,
        children: [
          {
            path: '',
            pathMatch: 'full',
            component: EditorSettingsGeneralTab,
          },
          {
            path: 'profiles/create',
            component: EditorSettingsPdfProfileCreateTab,
          },
          {
            path: 'profiles/edit/:profileId',
            component: EditorSettingsPdfProfileEditTab,
          },
          {
            path: 'profiles',
            component: EditorSettingsPdfProfilesTab,
          },
          {
            path: '**',
            redirectTo: '',
          },
        ],
      },
      {
        path: '**',
        redirectTo: 'cii',
      },
    ],
  },
];

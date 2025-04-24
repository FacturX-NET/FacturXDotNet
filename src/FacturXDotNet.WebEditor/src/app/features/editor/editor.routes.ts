import { Routes } from '@angular/router';
import { EditorLayout } from './editor.layout';
import { XmpTab } from './editor-tabs/editor-xmp/xmp.tab';
import { CiiTab } from './editor-tabs/editor-cii/cii.tab';
import { AttachmentsTab } from './editor-tabs/editor-attachments/attachments.tab';
import { EditorSettingsTab } from './editor-tabs/editor-settings/editor-settings.tab';
import { EditorSettingsGeneralTab } from './editor-tabs/editor-settings/editor-settings-tabs/editor-settings-general/editor-settings-general.tab';
import { EditorSettingsPdfProfilesTab } from './editor-tabs/editor-settings/editor-settings-tabs/editor-settings-pdf-profiles/editor-settings-pdf-profiles.tab';
import { EditorSettingsPdfProfileCreateTab } from './editor-tabs/editor-settings/editor-settings-tabs/editor-settings-pdf-profiles/editor-settings-pdf-profile-create.tab';
import { EditorSettingsPdfProfileEditTab } from './editor-tabs/editor-settings/editor-settings-tabs/editor-settings-pdf-profiles/editor-settings-pdf-profile-edit.tab';
import { resetPdfProfileOverride } from './guards/reset-pdf-profile-override';
import { EditorWelcomePage } from './editor-welcome.page';
import { EditorPage } from './editor.page';

export const routes: Routes = [
  {
    path: '',
    component: EditorLayout,
    children: [
      {
        path: 'welcome',
        component: EditorWelcomePage,
      },
      {
        path: '',
        component: EditorPage,
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'cii',
          },
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
                canDeactivate: [resetPdfProfileOverride],
              },
              {
                path: 'profiles/edit/:profileId',
                component: EditorSettingsPdfProfileEditTab,
                canDeactivate: [resetPdfProfileOverride],
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
        ],
      },
      {
        path: '**',
        redirectTo: 'cii',
      },
    ],
  },
];

import { Component, inject } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-basic',
  imports: [FormsModule, ReactiveFormsModule],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => {
        return inject(ControlContainer, { skipSelf: true });
      },
    },
  ],
  template: `
    <div class="card" formGroupName="basic">
      <div class="card-body">
        <h4 class="card-title sticky-top bg-body">Basic XMP metadata</h4>
        <p id="basic" class="card-text">The XMP basic namespace contains properties that provide basic descriptive information.</p>

        <div class="row">
          <div class="col">
            <label class="form-label" for="basicIdentifier">Identifier</label>
            <input id="basicIdentifier" class="editor__control form-control" formControlName="identifier" aria-describedby="basicIdentifierHelp" />
            <p id="basicIdentifierHelp" class="form-text">
              Unambiguously identify the resource within a given context. An array item may be qualified with xmpidq:Scheme to denote the formal identification system to which that
              identifier conforms.
            </p>
          </div>

          <div class="col">
            <label class="form-label" for="basicLabel">Label</label>
            <input id="basicLabel" class="editor__control form-control" formControlName="label" aria-describedby="basicLabelHelp" />
            <p id="basicLabelHelp" class="form-text">A word or short phrase that identifies a resource as a member of a userdefined collection.</p>
          </div>
        </div>

        <div class="row">
          <div class="col">
            <label class="form-label" for="basicCreateDate">Creation date</label>
            <input id="basicCreateDate" class="editor__control form-control" formControlName="createDate" type="datetime-local" aria-describedby="basicCreateDateHelp" />
            <p id="basicCreateDateHelp" class="form-text">
              The date and time the resource was created. For a digital file, this need not match a file-system creation time. For a freshly created resource, it should be close to
              that time, modulo the time taken to write the file. Later file transfer, copying, and so on, can make the file-system time arbitrarily different.
            </p>
          </div>

          <div class="col">
            <label class="form-label" for="basicModifyDate">Modification date</label>
            <input id="basicModifyDate" class="editor__control form-control" formControlName="modifyDate" type="datetime-local" aria-describedby="basicModifyDateHelp" />
            <p id="basicModifyDateHelp" class="form-text">
              The date and time the resource was last modified. NOTE: The value of this property is not necessarily the same as the file’s system modification date because it is
              typically set before the file is saved.
            </p>
          </div>

          <div class="col">
            <label class="form-label" for="basicMetadataDate">Metadata date</label>
            <input id="basicMetadataDate" class="editor__control form-control" formControlName="metadataDate" type="datetime-local" aria-describedby="basicMetadataDateHelp" />
            <p id="basicMetadataDateHelp" class="form-text">
              The date and time that any metadata for this resource was last changed. It should be the same as or more recent than xmp:ModifyDate.
            </p>
          </div>
        </div>

        <div class="mb-3">
          <label class="form-label" for="basicRating">Rating</label>
          <input id="basicRating" class="editor__control form-control" formControlName="rating" type="number" min="-1" max="5" aria-describedby="basicRatingHelp" />
          <p id="basicRatingHelp" class="form-text">
            A user-assigned rating for this file. The value shall be -1 or in the range [0..5], where -1 indicates “rejected” and 0 indicates “unrated”. If xmp:Rating is not
            present, a value of 0 should be assumed.
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="basicBaseUrl">Base URL</label>
          <input id="basicBaseUrl" class="editor__control form-control" formControlName="baseUrl" aria-describedby="basicBaseUrlHelp" />
          <p id="basicBaseUrlHelp" class="form-text">
            The base URL for relative URLs in the document content. If this document contains Internet links, and those links are relative, they are relative to this base URL. This
            property provides a standard way for embedded relative URLs to be interpreted by tools. Web authoring tools should set the value based on their notion of where URLs
            will be interpreted.
          </p>
        </div>

        <div>
          <label class="form-label" for="basicNickname">Nickname</label>
          <input id="basicNickname" class="editor__control form-control" formControlName="nickname" aria-describedby="basicNicknameHelp" />
          <p id="basicNicknameHelp" class="form-text">A short informal name for the resource.</p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="basicCreatorTool">Creator tool</label>
          <input id="basicCreatorTool" class="editor__control form-control" formControlName="creatorTool" aria-describedby="basicCreatorToolHelp" />
          <p id="basicCreatorToolHelp" class="form-text">The name of the first known tool used to create the resource.</p>
        </div>
      </div>
    </div>
  `,
})
export class XmpFormBasicComponent {}

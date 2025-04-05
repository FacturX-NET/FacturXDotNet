import { Component, inject } from '@angular/core';
import { ControlContainer, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-xmp-form-dublin-core',
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
    <div class="card" formGroupName="dublinCore">
      <div class="card-body">
        <h4 id="dublin-core" class="card-title">Dublin Core</h4>
        <p class="card-text">
          The Dublin Core namespace provides a set of commonly used properties. The names and usage shall be as defined in the Dublin Core Metadata Element Set, created by the
          Dublin Core Metadata Initiative (DCMI).
        </p>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreContributor">Contributor</label>
          <input id="dublinCoreContributor" class="editor__control form-control" formControlName="contributor" aria-describedby="dublinCoreContributorHelp" />
          <p id="dublinCoreContributorHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: An entity responsible for making contributions to the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Examples of a contributor include a person, an organization, or a service. Typically, the name of a contributor should be
            used to indicate the entity. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of contributors. These contributors should not include those listed in dc:creator.
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreCoverage">Coverage</label>
          <input id="dublinCoreCoverage" class="editor__control form-control" formControlName="coverage" aria-describedby="dublinCoreCoverageHelp" />
          <p id="dublinCoreCoverageHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under
            which the resource is relevant. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is the extent or scope of the resource. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreCreator">Creator</label>
          <input id="dublinCoreCreator" class="editor__control form-control" formControlName="creator" aria-describedby="dublinCoreCreatorHelp" />
          <p id="dublinCoreCreatorHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: An entity primarily responsible for making the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Examples of a creator include a person, an organization, or a service. Typically, the name of a creator should be used to
            indicate the entity. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of creators. Entities should be listed in order of decreasing precedence, if such order is
            significant. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreDate">Date</label>
          <input id="dublinCoreDate" class="editor__control form-control" formControlName="date" type="date" aria-describedby="dublinCoreDateHelp" />
          <p id="dublinCoreDateHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: A point or period of time associated with an event in the life cycle of the resource.
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreDescription">Description</label>
          <input id="dublinCoreDescription" class="editor__control form-control" formControlName="description" aria-describedby="dublinCoreDescriptionHelp" />
          <p id="dublinCoreDescriptionHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: An account of the resource. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of textual descriptions of the content of the resource, given in various languages. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreFormat">Format</label>
          <input id="dublinCoreFormat" class="editor__control form-control" formControlName="format" aria-describedby="dublinCoreFormatHelp" />
          <p id="dublinCoreFormatHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: The file format, physical medium, or dimensions of the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Examples of dimensions include size and duration. Recommended best practice is to use a controlled vocabulary such as the
            list of Internet Media Types [MIME]. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a MIME type. Dimensions would be stored using a media-specific property, beyond the scope of this document.
            <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreIdentifier">Identifier</label>
          <input id="dublinCoreIdentifier" class="editor__control form-control" formControlName="identifier" aria-describedby="dublinCoreIdentifierHelp" />
          <p id="dublinCoreIdentifierHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: An unambiguous reference to the resource within a given context. <br />
            <span class="fw-semibold">DCMI comment</span>: Recommended best practice is to identify the resource by means of a string conforming to a formal identification system.
            <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreLanguage">Language</label>
          <input id="dublinCoreLanguage" class="editor__control form-control" formControlName="language" aria-describedby="dublinCoreLanguageHelp" />
          <p id="dublinCoreLanguageHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: A language of the resource. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of languages used in the content of the resource. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCorePublisher">Publisher</label>
          <input id="dublinCorePublisher" class="editor__control form-control" formControlName="publisher" aria-describedby="dublinCorePublisherHelp" />
          <p id="dublinCorePublisherHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: An entity responsible for making the resource available. <br />
            <span class="fw-semibold">DCMI comment</span>: Examples of a publisher include a person, an organization, or a service. Typically, the name of a publisher should be
            used to indicate the entity. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of publishers. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreRelation">Relation</label>
          <input id="dublinCoreRelation" class="editor__control form-control" formControlName="relation" aria-describedby="dublinCoreRelationHelp" />
          <p id="dublinCoreRelationHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: A related resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Recommended best practice is to identify the related resource by means of a string conforming to a formal identification
            system. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of related resources. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreRights">Rights</label>
          <input id="dublinCoreRights" class="editor__control form-control" formControlName="rights" aria-describedby="dublinCoreRightsHelp" />
          <p id="dublinCoreRightsHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: Information about rights held in and over the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Typically, rights information includes a statement about various property rights associated with the resource, including
            intellectual property rights. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of informal rights statements, given in various languages. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreSource">Source</label>
          <input id="dublinCoreSource" class="editor__control form-control" formControlName="source" aria-describedby="dublinCoreSourceHelp" />
          <p id="dublinCoreSourceHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: A related resource from which the described resource is derived. <br />
            <span class="fw-semibold">DCMI comment</span>: The described resource may be derived from the related resource in whole or in part. Recommended best practice is to
            identify the related resource by means of a string conforming to a formal identification system. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreSubject">Subject</label>
          <input id="dublinCoreSubject" class="editor__control form-control" formControlName="subject" aria-describedby="dublinCoreSubjectHelp" />
          <p id="dublinCoreSubjectHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: The topic of the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Typically, the subject will be represented using keywords, key phrases, or classification codes. Recommended best
            practice is to use a controlled vocabulary. To describe the spatial or temporal topic of the resource, use the dc:coverage element. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a list of descriptive phrases or keywords that specify the content of the resource. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreTitle">Title</label>
          <input id="dublinCoreTitle" class="editor__control form-control" formControlName="title" aria-describedby="dublinCoreTitleHelp" />
          <p id="dublinCoreTitleHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: A name given to the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Typically, a title will be a name by which the resource is formally known. <br />
            <span class="fw-semibold">XMP addition</span>: XMP usage is a title or name, given in various languages. <br />
          </p>
        </div>

        <div class="mb-3">
          <label class="form-label" for="dublinCoreType">Type</label>
          <input id="dublinCoreType" class="editor__control form-control" formControlName="type" aria-describedby="dublinCoreTypeHelp" />
          <p id="dublinCoreTypeHelp" class="form-text">
            <span class="fw-semibold">DCMI definition</span>: The nature or genre of the resource. <br />
            <span class="fw-semibold">DCMI comment</span>: Recommended best practice is to use a controlled vocabulary such as the DCMI Type Vocabulary [DCMITYPE]. To describe the
            file format, physical medium, or dimensions of the resource, use the dc:format element. <br />
            <span class="fw-semibold">XMP addition</span>: See the dc:format entry for clarification of the XMP usage of that element. <br />
          </p>
        </div>
      </div>
    </div>
  `,
})
export class XmpFormDublinCoreComponent {}

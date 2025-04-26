export interface Sbom {
  readonly version: number;
  readonly metadata: {
    readonly component: SbomComponent;
  };
  readonly components: SbomComponent[];
  readonly dependencies: { readonly ref: string; readonly dependsOn: string[] }[];
}

export interface SbomComponent {
  readonly 'bom-ref': string;
  readonly name: string;
  readonly version: string;
  readonly author?: string;
  readonly description?: string;
  readonly licenses?: SbomLicense[];
  readonly externalReferences: SbomExternalReference[];
}

export type SbomLicense = SbomLicenseId | SbomLicenseExpression;

export interface SbomLicenseId {
  readonly license: { readonly id: string };
}

export interface SbomLicenseExpression {
  readonly expression: string;
}

export function isSbomLicenseExpression(license: SbomLicense): license is SbomLicenseExpression {
  return Object.keys(license).includes('expression');
}

export interface SbomExternalReference {
  readonly type: string;
  readonly url: string;
}

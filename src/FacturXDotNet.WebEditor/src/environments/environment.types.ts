export interface Environment {
  version: string;
  buildDate: Date;
  isUnsafeCloudEnvironment?: boolean;
  apiUrl: string;
}

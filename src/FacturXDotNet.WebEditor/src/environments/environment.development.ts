import { Environment } from './environment.types';

export const environment: Environment = {
  version: '~dev',
  buildDate: new Date(),
  isUnsafeCloudEnvironment: true,
  apiUrl: 'http://localhost:5295',
};

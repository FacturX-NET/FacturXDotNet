export function downloadFile(file: File, name?: string) {
  downloadBlob(file, name ?? file.name);
}

export function downloadBlob(blob: Blob, name?: string) {
  const blobUrl = window.URL.createObjectURL(blob);

  const a = document.createElement('a') as HTMLAnchorElement;
  a.href = blobUrl;
  a.download = name ?? blobUrl;

  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
}

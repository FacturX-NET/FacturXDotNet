import fs from "node:fs";

export default {
  watch: ["./env.json"],
  load(watchedFile) {
    const file = watchedFile[0];
    const fileContent = fs.readFileSync(file, "utf-8");
    return JSON.parse(fileContent) as Env;
  },
};

interface Env {
  buildName: string;
  version: string;
  editor: {
    url: string;
  };
  api: {
    url: string;
  };
}

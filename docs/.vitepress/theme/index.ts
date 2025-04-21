import DefaultTheme from "vitepress/theme";

import vitepressNprogress from "vitepress-plugin-nprogress";
import "vitepress-plugin-nprogress/lib/css/index.css";

import {theme} from "vitepress-openapi/client";
import "vitepress-openapi/dist/style.css";

import "./custom.css";

export default {
  ...DefaultTheme,

  enhanceApp(ctx) {
    theme.enhanceApp(ctx);
    return vitepressNprogress(ctx);
  },
};

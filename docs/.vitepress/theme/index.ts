import {h} from "vue";
import DefaultTheme from "vitepress/theme";

import vitepressNprogress from "vitepress-plugin-nprogress";
import "vitepress-plugin-nprogress/lib/css/index.css";

import {theme} from "vitepress-openapi/client";
import "vitepress-openapi/dist/style.css";

import RegisterSW from "./components/RegisterSW.vue";

import "./custom.css";

export default {
  ...DefaultTheme,

  layout() {
    return h(DefaultTheme.Layout, null, {
      "layout-bottom": () => h(RegisterSW),
    });
  },

  enhanceApp(ctx) {
    theme.enhanceApp(ctx);
    return vitepressNprogress(ctx);
  },
};

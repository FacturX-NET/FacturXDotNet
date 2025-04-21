import DefaultTheme from "vitepress/theme";

import vitepressNprogress from "vitepress-plugin-nprogress";
import "vitepress-plugin-nprogress/lib/css/index.css";

import {theme} from "vitepress-openapi/client";
import "vitepress-openapi/dist/style.css";

import "viewerjs/dist/viewer.min.css";
import imageViewer from "vitepress-plugin-image-viewer";
import {useRoute} from "vitepress";

import "./custom.css";

export default {
  ...DefaultTheme,

  enhanceApp(ctx) {
    theme.enhanceApp(ctx);
    return vitepressNprogress(ctx);
  },
  setup() {
    // Get route
    const route = useRoute();
    // Using
    imageViewer(route);
  },
};

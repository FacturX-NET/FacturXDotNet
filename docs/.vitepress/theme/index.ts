import {h} from "vue";
import DefaultTheme from "vitepress/theme";
import RegisterSW from "./components/RegisterSW.vue";

import "./custom.css";

export default {
  ...DefaultTheme,
  layout() {
    return h(DefaultTheme.Layout, null, {
      "layout-bottom": () => h(RegisterSW),
    });
  },
};

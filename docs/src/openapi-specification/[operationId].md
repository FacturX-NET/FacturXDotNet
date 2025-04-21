---
title: OpenAPI specification
pageClass: openapi-specification-page
aside: false
outline: false
editLink: false
prev: false
next: false
---

<script setup lang="ts">
import { useRoute } from 'vitepress'
import spec from '../assets/facturxdotnet.openapi.json'

const route = useRoute();

const operationId = route.data.params.operationId;
</script>

<OAOperation :spec="spec" :operationId="operationId">
<template #branding>
</template>
</OAOperation>
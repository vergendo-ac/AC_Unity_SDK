### How to draw diagrams:

* VS code PlantUML

*   ```shell
    cat flow.uml | docker run --rm -i think/plantuml -tpng > flow.png
    ````

### OpenAPI

https://developer.augmented.city/doc#operation/localize


### Generate proxy

```shell
npx openapi-generator generate -i swagger.json -g csharp-netcore -o ./unity_sdk/client/
```

```shell
npx openapi-generator generate -i swagger.json -g kotlin -o ./android_sdk/client/
```

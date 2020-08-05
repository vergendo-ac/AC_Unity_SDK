### How to draw diagrams:

* VS code PlantUML

*   ```shell
    cat flow.uml | docker run --rm -i think/plantuml -tpng > flow.png
    ````

### OpenAPI

https://developer.augmented.city/doc#operation/localize


### Generate proxy

```shell
npx openapi-generator generate -i api.yaml -g csharp-netcore -o ./netcore
```

Run test
```shell
dotnet test --filter "Org.OpenAPITools.Test.VersionApiTests.ServerVersionGetTest"
```
Pasos para generar el paquete nuget:

1. Agregar en assembly info los siguientes datos:
   * AssemblyTitle
   * AssemblyDescription
   * AssemblyCompany
   * AssemblyProduct
   * AssemblyCopyright
   * AssemblyInformationalVersion

2. Generar el archivo .nuspec

    ```
    nuget pack of.data.csproj -IncludeReferencedProjects
    ```

3. Editar el archivo .nuspec para agregar .pdb u otras especificaciones.

4. Agregar evento post-build para hacer el paquete nuget. Cada vez que se compile el proyecto, se generará el paquete nuget con la versión correspondiente.

    ```
    nuget pack ..\..\$(ProjectName).csproj -IncludeReferencedProjects
    ```

5. Subir paquete nuget

    ```
    nuget push .\bin\debug\{title}.1.0.0.nupkg {key} -Source http://{nuget-server}/api/v2/package
    ```

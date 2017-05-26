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
    nuget spec of.data
    ```

3. Editar el archivo .nuspec para agregar .pdb u otras especificaciones. Ejemplo:

    ``` xml
    <?xml version="1.0"?>
    <package >
        <metadata>
            <id>$id$</id>
            <version>$version$</version>
            <title>$title$</title>
            <authors>$author$</authors>
            <owners>$author$</owners>
            <!--    <licenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</licenseUrl>-->
            <!--    <projectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</projectUrl>-->
            <!--    <iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>-->
            <requireLicenseAcceptance>false</requireLicenseAcceptance>
            <description>$description$</description>
            <!--    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>-->
            <copyright>$copyright$</copyright>
            <tags>$description$</tags>
        </metadata>
        <files>
            <file src="bin\$configuration$\$id$.pdb" target="lib\net461\" />
        </files>
    </package>
    ```

4. Agregar evento post-build para hacer el paquete nuget. Cada vez que se compile el proyecto, se generará el paquete nuget con la versión correspondiente.

    ```
    nuget pack ..\..\$(ProjectName).csproj -IncludeReferencedProjects -OutputDirectory ..\..\
    ```

5. Subir paquete nuget

    ```
    nuget push .\bin\debug\{title}.1.0.0.nupkg {key} -Source http://{nuget-server}/api/v2/package
    ```

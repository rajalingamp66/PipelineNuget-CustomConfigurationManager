import groovy.transform.Field

@Field String newTag = ''

pipeline {
    agent { label 'doa-injwn01.in.lab' }

    parameters {
        gitParameter(
            branchFilter: 'origin/(.*)',
            defaultValue: 'main_1.0',
            name: 'BRANCH',
            type: 'PT_BRANCH',
            listSize: '10',
            quickFilterEnabled: true,
            useRepository: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager'
        )
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/inContact/index.json"
        GITHUB_USERNAME = "Rajalingam.Periyathambi@nice.com"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {

        stage('Checkout') {
            steps {
                git credentialsId: 'github-nice-cxone',
                    url: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager.git',
                    branch: "${params.BRANCH}"
            }
        }

        stage("Preparation: Generate New Tag") {
            steps {
                script {
                    echo "Generating new tag for branch ${params.BRANCH} with release type ${params.RELEASE_TYPE}"
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${params.RELEASE_TYPE} ${params.BRANCH}").trim()
                    echo "✅ New Tag: ${newTag}"
                }
            }
        }

        stage('Build & Pack NuGet') {
            steps {
                script {
                    echo '🔧 Building and packing the NuGet package...'
                    withEnv(["newTag=${newTag}"]) {
                        bat '''
                            cd src
                            echo "Restoring and building solution..."
                            dotnet restore PipelineNuget-CustomConfigurationManager.sln --configfile .nuget\\NuGet.Config
                            dotnet build PipelineNuget-CustomConfigurationManager.sln --configfile .nuget\\NuGet.Config

                            echo "Packing NuGet with version %newTag%..."
                            dotnet pack -c Release -p:Version=%newTag% PipelineNuget-CustomConfigurationManager.sln
                        '''
                    }
                }
            }
        }

        stage('Push to GitHub NuGet Feed') {
            steps {
                script {
                    echo '📦 Pushing NuGet package to GitHub Packages...'
                    bat '''
                        for %%F in (src\\**\\bin\\Release\\*.nupkg) do (
                            echo "Pushing package: %%F"
                            dotnet nuget push "%%F" -k %GITHUB_TOKEN% -s %NUGET_SOURCE% --skip-duplicate
                        )
                    '''
                }
            }
        }

        stage('Push Git Tag') {
            steps {
                script {
                    echo "🏷️ Pushing tag '${newTag}' to GitHub..."
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        bat '''
                            git config user.email "jenkins@vj-linux"
                            git config user.name "jenkins"
                            git config --local credential.helper "!f() { echo username=\\%GIT_USERNAME%; echo password=\\%GIT_PASSWORD%; }; f"

                            echo "Creating git tag %newTag%..."
                            git tag -a %newTag% -m "jenkins ci auto commit"

                            echo "Pushing tag..."
                            git push origin refs/tags/%newTag%
                        '''
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
            script {
                currentBuild.description = "${params.RELEASE_TYPE} : ${newTag}"
                echo "🧹 Workspace cleaned. Build description set to: ${currentBuild.description}"
            }
        }
    }
}

import groovy.transform.Field

@Field String newTag=''

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
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select release type')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/rajalingamp66/index.json"
        GITHUB_USERNAME = "Rajalingam.Periyathambi@nice.com"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {
        stage("Resolve Version Tag") {
            steps {
                script {
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${RELEASE_TYPE} ${BRANCH}").trim()
                    echo "Branch: ${BRANCH}"
                    echo "Generated New Tag: ${newTag}"
                }
            }
        }

        stage("Build & Pack") {
            steps {
                script {
                    echo "Building CustomConfigurationManager"
                    withEnv(["newTag=${newTag}"]) {
                        bat '''
                            cd ./CustomConfigurationManager
                            dotnet build --configfile .nuget/NuGet.Config CustomConfigurationManager.sln
                            echo "Packing NuGet with version %newTag%"
                            dotnet pack -c Release -p:Version=%newTag%
                        '''
                    }
                }
            }
        }

        stage("Publish to NuGet") {
            steps {
                script {
                    bat '''
                        dotnet nuget push CustomConfigurationManager/bin/Release/*.nupkg -k %GITHUB_TOKEN% -s %NUGET_SOURCE%
                    '''
                }
            }
        }

        stage("Push Git Tag") {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        withEnv(["newTag=${newTag}"]) {
                            bat '''
                                git config --local credential.helper "!f() { echo username=\\%GIT_USERNAME%; echo password=\\%GIT_PASSWORD%; }; f"
                                git config user.email "jenkins@vj-linux"
                                git config user.name "jenkins"
                                git tag -a %newTag% -m "Jenkins auto tag"
                                git push origin refs/tags/%newTag%
                            '''
                        }
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
            script {
                currentBuild.description = "${RELEASE_TYPE} → ${newTag}"
            }
        }
    }
}

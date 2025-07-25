import groovy.transform.Field

@Field String newTag=''

pipeline {
    agent { label 'doa-injwn01.in.lab' }

    parameters {
        gitParameter branchFilter: 'origin/(.*)', defaultValue: 'PDO-9290', name: 'BRANCH', type: 'PT_BRANCH', listSize: '10', quickFilterEnabled: true, useRepository: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager'
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/inContact/index.json"
        GITHUB_USERNAME = "Rajalingam.Periyathambi@nice.com"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {
        stage("Preparation Stage") {
            steps {
                script {
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${RELEASE_TYPE} ${BRANCH}").trim()
                    println "Branch name: ${BRANCH}"
                    println "New Tag generated: ${newTag}"
                }
            }
        }

        stage('Build') {
            steps {
                script {
                    echo 'Building the NuGet package'
                    withEnv(["newTag=${newTag}"]) {
                        bat '''
                            cd ./src
                            dotnet build --configfile .nuget/NuGet.Config PipelineNuget-CustomConfigurationManager.sln
                            
                            echo 'Packing the build'
                            dotnet pack PipelineNuget-CustomConfigurationManager/PipelineNuget-CustomConfigurationManager.csproj -c Release -p:Version=%newTag%
                        '''
                    }
                    echo 'Push package to GitHub NuGet feed'
                    bat '''
                        dotnet nuget push src/PipelineNuget-CustomConfigurationManager/bin/Release/PipelineNuget-CustomConfigurationManager.%newTag%.nupkg -k %GITHUB_TOKEN% -s %NUGET_SOURCE%
                    '''
                }
            }
        }

        stage('Push Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        withEnv(["newTag=${newTag}"]) {
                            bat '''
                                echo Tagging version: %newTag%

                                git config --local credential.helper "!f() { echo username=%GIT_USERNAME%; echo password=%GIT_PASSWORD%; }; f"
                                git config user.email "jenkins@vj-linux"
                                git config user.name "jenkins"

                                git tag -m "jenkins ci auto commit" -a %newTag%
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
                currentBuild.description = "${RELEASE_TYPE} : ${newTag}"
            }
        }
    }
}

import groovy.transform.Field

@Field String newTag = ''

pipeline {
    agent { label 'doa-injwn01.in.lab' }

    parameters {
        gitParameter branchFilter: 'origin/(.*)', defaultValue: 'main', name: 'BRANCH', type: 'PT_BRANCH', listSize: '10', quickFilterEnabled: true, useRepository: 'https://github.com/rajalingamp66/PipelineNuget-CustomConfigurationManager.git'
        choice(name: 'RELEASE_TYPE', choices: ['major', 'minor'], description: 'Select the release type for build')
    }

    environment {
        NUGET_SOURCE = "https://nuget.pkg.github.com/rajalingamp66/index.json"
        GITHUB_USERNAME = "rajalingamp66"
        GITHUB_TOKEN = credentials("github-packages-read-write")
    }

    stages {
        stage("Preparation Stage") {
            steps {
                script {
                    newTag = powershell(returnStdout: true, script: ".\\resolveGitChanges.ps1 ${RELEASE_TYPE} ${BRANCH}").trim()
                    echo "Branch name: ${BRANCH}"
                    echo "New Tag generated: ${newTag}"
                }
            }
        }

        stage('Build') {
            steps {
                script {
                    echo 'Building the package'
                    withEnv(["newTag=${newTag}"]) {
                        bat '''
                            cd ./src
                            dotnet build --configfile ../.nuget/NuGet.Config PipelineNuget-CustomConfigurationManager.sln
                            echo 'Packing the build'
                            dotnet pack -c Release -p:Version=%newTag% PipelineNuget-CustomConfigurationManager.sln
                        '''
                    }
                }
            }
        }

        stage('Push Package') {
            steps {
                script {
                    echo 'Pushing package to GitHub NuGet feed'
                    bat '''
                        for %%f in (src\\**\\bin\\Release\\*.nupkg) do (
                            dotnet nuget push "%%f" -k %GITHUB_TOKEN% -s %NUGET_SOURCE% --skip-duplicate
                        )
                    '''
                }
            }
        }

        stage('Push Git Tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-nice-cxone', usernameVariable: 'GIT_USERNAME', passwordVariable: 'GIT_PASSWORD')]) {
                        withEnv(["newTag=${newTag}"]) {
                            bat '''
                                git config --local credential.helper "!f() { echo username=%GIT_USERNAME%; echo password=%GIT_PASSWORD%; }; f"
                                git config user.email "jenkins@vj-linux"
                                git config user.name "jenkins"

                                git tag -a %newTag% -m "jenkins ci auto commit"
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

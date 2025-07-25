param (
  [string]$release,
  [string]$branchName
)

function createNewVersion([string]$branchName, [string]$release, [string]$latestVersion) {
  $latestVersion = $latestVersion.Replace("-alpha", "")
  $versionComponents = $latestVersion -split "\."

  if ($branchName -eq "main" -or $branchName -eq "main_1.0") {
    if ($release -eq "major") {
      $versionComponents[0] = [int]$versionComponents[0] + 1
      $versionComponents[1] = 0
      $versionComponents[2] = 0
    } else {
      $versionComponents[1] = [int]$versionComponents[1] + 1
      $versionComponents[2] = 0
    }
    $newVersion = "$($versionComponents[0]).$($versionComponents[1]).$($versionComponents[2])"
  } else {
    if ($release -eq "major") {
      $versionComponents[1] = [int]$versionComponents[1] + 1
      $newVersion = "$($versionComponents[0]).$($versionComponents[1]).0-alpha"
    } else {
      $versionComponents[2] = [int]$versionComponents[2] + 1
      $newVersion = "$($versionComponents[0]).$($versionComponents[1]).$($versionComponents[2])-alpha"
    }
  }

  return $newVersion
}

[string]$currentVersion = "0.0.0"

try {
  $latestTag = git describe --tags (git rev-list --tags --max-count=1)
  if ($latestTag) {
    Write-Host "Latest tag: $latestTag"
    $latestTagCommit = (git rev-list -n 1 $latestTag)
    $currentVersion = $latestTag
  } else {
    Write-Host "No tags found. Starting with version 0.0.0"
  }
} catch {
  Write-Host "Error retrieving latest tag. Using default version 0.0.0"
}

[string]$HEAD = (git rev-parse HEAD)
Set-Content -Path "changes.log" -Value "HEAD_COMMIT=$HEAD"
Add-Content -Path "changes.log" -Value "latestTagCommit=$latestTagCommit"

[Boolean]$buildChanged = $HEAD.Trim() -ne $latestTagCommit.Trim()

if ($buildChanged) {
  Add-Content -Path "changes.log" -Value "component has been changed"
  $newVersion = createNewVersion -branchName $branchName -release $release -latestVersion $currentVersion
  Write-Host "New version: $newVersion"
} else {
  $newVersion = $currentVersion
  Write-Host "No changes. Keeping version: $newVersion"
}

Write-Output "${newVersion}"

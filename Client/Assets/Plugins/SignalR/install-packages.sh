# Creating a temp folder
WORK_DIR=`mktemp -d`
if [[ ! "$WORK_DIR" || ! -d "$WORK_DIR" ]]; then
  echo "Could not create temp dir ${WORK_DIR}"
  exit 1
fi
echo "Created temp dir ${WORK_DIR}"

# register the cleanup action to be called on the EXIT signal
trap '{ rm -rf "$WORK_DIR"; }' EXIT

# Install SignalR and all dependecies into the temp folder
nuget install Microsoft.AspNetCore.SignalR.Client -OutputDirectory "$WORK_DIR" -Framework 'netstandard2.0'
nuget install Microsoft.AspNetCore.SignalR.Protocols.NewtonsoftJson -OutputDirectory "$WORK_DIR" -Framework 'netstandard2.0'


# find only .dll in the temp folder and copy it accross to the current directory
find "$WORK_DIR" -name 'netstandard2.0' -exec find {} -name '*.dll' \; | xargs -i cp -fv {} ./

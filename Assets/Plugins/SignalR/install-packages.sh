TEMPL='/tmp/card-game-nugets'
mkdir -p $TEMPL

if [[ ! "$TEMPL" || ! -d "$TEMPL" ]]; then
  echo "Could not create template dir"
  exit 1
fi

WORK_DIR=`mktemp -d -p $TEMPL`
TMPWORKDIR=$(basename $WORK_DIR)
if [[ ! "$WORK_DIR" || ! -d "$WORK_DIR" ]]; then
  echo "Could not create temp dir"
  exit 1
fi

# deletes the temp directory
function cleanup {   
  rm -r "${TEMPL}/${TMPWORKDIR}/*"
  rmdir "${TEMPL}/${TMPWORKDIR}"
  echo "Deleted temp working directory ${TEMPL}/${TMPWORKDIR}"
}

# register the cleanup function to be called on the EXIT signal
trap cleanup EXIT

nuget install Microsoft.AspNetCore.SignalR.Client -OutputDirectory "$WORK_DIR" -Framework 'netstandard2.0'
# nuget install System.IO -OutputDirectory './tmp' -Framework 'netstandard2.0'
find "$WORK_DIR" -name 'netstandard2.0' -exec find {} -name '*.dll' \; | xargs -i cp -fv {} ./

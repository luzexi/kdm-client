rm -rf ../../kdm-client/*
cp -r ./* ../../kdm-client/
cd ../../kdm-client/
git add .
git commit -m 'sync'
git push origin master

from pymongo import MongoClient
from flask import Flask, request
from flask_restful import Resource, Api
from sqlalchemy import create_engine
from json import dumps,loads
from flask.ext.jsonpify import jsonify
from bson import json_util, ObjectId
from math import radians, cos, sin, asin, sqrt

client = MongoClient('mongodb://test:test@ds123725.mlab.com:23725/ar')
db = client['ar']
collectionLocations = db['Locations']
collectionUsers = db['Users']

app = Flask(__name__)
api = Api(app)

class Locations(Resource):
    def get(self,latitude=None,longtitude=None,filter=None):
        locations = None
        if filter is None :
            locations = collectionLocations.find().sort('Category')
        else :
            filters = filter.split(',')
            locations = collectionLocations.find({'$or': [{'Category':aFilter} for aFilter in filters]}).sort('Category')
        latitude = float(latitude)
        longtitude = float(longtitude)
        locations = list(locations)
        for location in locations[:] :
            distance = Locations.calculateHaversine(latitude,longtitude,location['Latitude'],location['Longtitude'])
            if distance > 1:
                locations.remove(location)
            else :
                location['distance'] = distance
        result = {'locations': [
            Locations.idToString(location) for location in locations]}
        return result
    def calculateHaversine(lat1, lon1, lat2, lon2):
        r = 6372.797560856
        lon1, lat1, lon2, lat2 = map(radians, [lon1, lat1, lon2, lat2])
        dlon = lon2 - lon1 
        dlat = lat2 - lat1
        a = sin(dlat/2)**2 + cos(lat1) * cos(lat2) * sin(dlon/2)**2
        c = 2 * asin(sqrt(a)) 
        return c * r
    def idToString(location):
        location['id'] = str(location['_id'])
        del location['_id']
        return loads(json_util.dumps(location))

class Rate(Resource):
    def post(self):
        username = request.form['Username']
        locationId = request.form['locationId']
        rating = float(request.form['Rating'])
        getRating = collectionLocations.find_one({'_id' : ObjectId(locationId)},{'Rating':1,'RatedUser':1})
        if username not in getRating['RatedUser'] :
            RatedAmount = len(getRating['RatedUser'])
            rating = (getRating['Rating']*RatedAmount+rating)/(RatedAmount+1)
            collectionLocations.update_one({'_id' : ObjectId(locationId)},{'$set':{'Rating':rating},'$push':{'RatedUser': username}})
        else :
            return "You've already rated this place!" 

class Comment(Resource):
    def post(self):
        comment = request.form['Comment']
        locationId = request.form['locationId']
        collectionLocations.update_one({'_id' : ObjectId(locationId)},{'$push':{'Comments': comment}})

class Report(Resource):
    def post(self):
        username = request.form['Username']
        type = request.form['Type']
        detail = request.form['Detail']
        locationId = request.form['locationId']
        collectionLocations.update_one({'_id' : ObjectId(locationId)},{'$push':{'Reports': {'Type':type,'Detail':detail,'ReportedBy':username}}})
        
class AddLocation(Resource):
    def post(self):
        name = request.form['Name']
        category = request.form['Category']
        latitude = float(request.form['Latitude'])
        longtitude = float(request.form['Longtitude'])
        urlPic = request.form['URLPic']
        collectionLocations.insert_one({
            'Name':name,
            'Category':category,
            'Latitude':latitude,
            'Longtitude':longtitude,
            'Rating':0,
            'RatedUser':[],
            'URLPic':urlPic,
            'Comments':[],
            'Reports':[]})

class Login(Resource):
    def post(self):
        collectionUsers.insert_one({
            'Username':request.form['Username']})

api.add_resource(Locations, '/search=<latitude>,<longtitude>','/search=<latitude>,<longtitude>&filter=<string:filter>')
api.add_resource(Rate, '/rate')
api.add_resource(Comment, '/comment')
api.add_resource(Report, '/report')
api.add_resource(AddLocation, '/addlocation')
api.add_resource(Login, '/login')

if __name__ == '__main__':
    app.run(host='161.200.203.190',debug=True)

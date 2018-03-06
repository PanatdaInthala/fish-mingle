Fish Mingle Specifications

1. First, a user should be able to create an account on fish mingle.
2. Once a user has created an account, a user should be able to create a profile that is linked to only their account.
3. Once a profile has been created, the user should be able to upload a photo to their profile for their profile picture.
4. Once the profile is created, the user should be able to edit their profile, and delete their profile if they like.
5. Then, when a user has a completed profile, the user should be able to be matched with other profiles.
6. Once a user has a profile, the user should be able to browse other profiles that are on the site.
7. When a user is viewing a profile, they should be able to like the profile.
8. The user should receive notifications when another user likes their profile(optional?lol)
9. When two users like each others profile, the users should be directed to a match page.

GetMatches() Query: SELECT B.* FROM users AS A JOIN users_users ON (A.id = users_users.fish1_id) JOIN users AS B ON (users_users.fish2_id = B.id) WHERE A.id = 1

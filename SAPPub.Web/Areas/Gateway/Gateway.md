# Gateway

To enable the Private MVP, a gateway service was considered. 

This feature will restrict all functionality/pages from being accessed unless the user has a cookie matching an Id in the postgres database. 
The existance of this cookie means the user has accepted the terms and conditions of the Private MVP.

## Flow

User gets link from LA in the form <site>/gateway/welcome/<council>

- The list of possible councils is stored in the controllers as a static list.
- User presented with welcome page, and two options, "I've already been here" and "I haven't been here before"

- If the council in the URL is not valid, user is shown an error message.
- If the council in the URL has reached its maximum number of users, user redirect to a closed page.

If user clicks "I've already been here", they are taken to the returning page, where they can enter their email address.

- Email is checked against the database, if it exists, a cookie is set with the corresponding Id and user is redirected to the home page.
- If email does not exist, user is shown an error informing them that their email is valid, but not found in the database, please try again.
- If email is invalid, user is shown an error to enter a valid email.

If user clicks "I've never been here before", they are taken to the registration page, where they can enter their email address, and a few other questions. 

- If email is valid, and does not exist in the database, a new record is created, a cookie is set with the new Id and user is redirected to the home page.
- If email is valid, and does not exist in the database, user is also sent an email with a link to the welcome page. 
- If email is invalid, user is shown an error to enter a valid email.
- If email is valid, and already exists in the database, user is shown an error that email already exists, with a link to go to returning page.

On each page request -
- the gateway checks for the cookie, and if it does not exist, or does not match an Id in the database, user is sent to an error page.
- the gateway also checks if the service should be closed, by querying the database for a global open/off switch. If the service is closed, user is redirected to a closed page.

On each registration "new user page" - we check if total signups is greater than total allowed (config in DB) or if total sign ups for LA is greater than allowed (config in DB)
	If either of these are true, then user presented page with "No more signups"

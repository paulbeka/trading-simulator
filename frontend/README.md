

# The Plan

We can't fully simulate getting live prices, so we'll fetch from Yahoo Finance the positions that we currently own for equities. 

Workflow:
- User choses to buy/sell assets on a dashboard. They can look them up with a searchbar.
- If they place an order, it's stored somewhere and bought at the price the order is executed.
- That goes into a positions database
- On the dashboard, the user can see their live P&L from different assets. 

Options:
- the user can buy options, which also has a P&L component. 
- The option is priced using black scholes etc (+ maybe using some other methods such as vol smiles to properly model the real prices)
- The option can then be re-sold or executed as necessary

